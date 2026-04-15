import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormArray, ReactiveFormsModule, Validators } from '@angular/forms';
import { FaturamentoService } from '../../core/services/faturamento.service';
import { NotaFiscalForRegistrationDTO } from '../../core/models/nota-fiscal-for-registration-dto.model';
import { NotaFiscalItemDTO } from '../../core/models/nota-fiscal-item-dto.model';

@Component({
    selector: 'app-faturamento',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule],
    templateUrl: './faturamento.component.html'
})
export class FaturamentoComponent implements OnInit {
    // Injeções
    private faturamentoService = inject(FaturamentoService);
    private fb = inject(FormBuilder);
    private cdr = inject(ChangeDetectorRef);

    // Estados Base
    faturamentoForm!: FormGroup;
    mensagem: { texto: string, tipo: 'sucesso' | 'erro' } | null = null;
    statusVisual = 'Aberta';

    ngOnInit(): void {
        // Inicializa o form vazio, porém com um array pronto para os sub-itens
        this.faturamentoForm = this.fb.group({
            itens: this.fb.array([])
        });

        // Insere automaticamente a primeira de linha produto no load da tela
        this.adicionarItem();
    }

    // Getter auxiliar para obter o FormArray como instância tipada
    get itensFormArray(): FormArray {
        return this.faturamentoForm.get('itens') as FormArray;
    }

    // Função fábrica para criar um novo "FormGroup" correspondente a uma linha
    novoItemFormGroup(): FormGroup {
        return this.fb.group({
            produtoCodigo: ['', Validators.required],
            quantidade: [1, [Validators.required, Validators.min(1)]]
        });
    }

    // Action: Incrementa nova linha na UI
    adicionarItem(): void {
        this.itensFormArray.push(this.novoItemFormGroup());
    }

    // Action: Remove uma linha da UI (impede que o usuário tire o último item)
    removerItem(index: number): void {
        if (this.itensFormArray.length > 1) {
            this.itensFormArray.removeAt(index);
        } else {
            this.exibirMensagem('A nota fiscal precisa ter no mínimo 1 item.', 'erro');
        }
    }

    // Action: Envia a requisição a API e reseta o sistema
    onSubmit(): void {
        // Tratativa extra forçando os erros a aparecerem em tela nos inputs
        if (this.faturamentoForm.invalid) {
            this.faturamentoForm.markAllAsTouched();
            this.exibirMensagem('Verifique os códigos e as quantidades inseridas.', 'erro');
            return;
        }

        // Bloqueio extra
        if (this.itensFormArray.length === 0) {
            this.exibirMensagem('Adicione itens na nota fiscal primeiro.', 'erro');
            return;
        }

        // Montando o DTO com Casting Direto pois nosso form respeita a mesma nomenclatura e tipagem
        const dto: NotaFiscalForRegistrationDTO = {
            itens: this.faturamentoForm.value.itens as NotaFiscalItemDTO[]
        };

        this.faturamentoService.gerarNotaFiscal(dto).subscribe({
            next: () => {
                // Limpa todas as linhas atuais e o form
                this.itensFormArray.clear();
                this.faturamentoForm.reset();

                // Retorna a ter uma (1) linha limpa pronta p/ uso novo
                this.adicionarItem();
                this.exibirMensagem('Nota fiscal # gerada e enviada com sucesso ao servidor.', 'sucesso');
                this.cdr.detectChanges();
            },
            error: (err) => {
                this.exibirMensagem(err.message, 'erro');
                this.cdr.detectChanges();
            }
        });
    }

    // Utilidade interna para feedbacks timeout
    exibirMensagem(texto: string, tipo: 'sucesso' | 'erro'): void {
        this.mensagem = { texto, tipo };
        this.cdr.detectChanges();
        setTimeout(() => {
            this.mensagem = null;
            this.cdr.detectChanges();
        }, 5000);
    }
}
