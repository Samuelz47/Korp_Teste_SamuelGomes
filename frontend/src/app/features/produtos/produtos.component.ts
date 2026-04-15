import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { EstoqueService } from '../../core/services/estoque.service';
import { Produto } from '../../core/models/produto.model';

@Component({
    selector: 'app-produtos',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule],
    templateUrl: './produtos.component.html'
})
export class ProdutosComponent implements OnInit {
    // Injeções de dependência
    private estoqueService = inject(EstoqueService);
    private fb = inject(FormBuilder);
    private cdr = inject(ChangeDetectorRef);

    // Estados da tela
    produtos: Produto[] = [];
    produtoForm: FormGroup;
    mensagem: { texto: string, tipo: 'sucesso' | 'erro' } | null = null;

    constructor() {
        // Configuração do formulário e de suas obrigatoriedades e tipos
        this.produtoForm = this.fb.group({
            codigo: ['', Validators.required],
            descricao: ['', Validators.required],
            saldo: [0, [Validators.required, Validators.pattern('^[0-9]+(\\.[0-9]{1,2})?$')]]
        });
    }

    // Ciclo de vida: executado ao carregar o componente local
    ngOnInit(): void {
        this.carregarProdutos();
    }

    // Atinge o microsserviço de listagem de estoque (Get)
    carregarProdutos(): void {
        this.estoqueService.obterProdutos().subscribe({
            next: (dados) => {
                this.produtos = dados;
                this.cdr.detectChanges();
            },
            error: (err) => {
                this.exibirMensagem(err.message, 'erro');
                this.cdr.detectChanges();
            }
        });
    }

    // Ação ativada com o submit do Formulário (Post)
    onSubmit(): void {
        if (this.produtoForm.invalid) {
            this.exibirMensagem('Por favor, preencha todos os campos obrigatórios corretamente.', 'erro');
            Object.keys(this.produtoForm.controls).forEach(key => {
                this.produtoForm.get(key)?.markAsTouched(); // Destacar os campos vazios no HTML
            });
            return;
        }

        const novoProduto: Produto = this.produtoForm.value;

        this.estoqueService.cadastrarProduto(novoProduto).subscribe({
            next: (produtoSalvo) => {
                // Se a API retornar o Objeto Inteiro usamos ele, se não usamos a referência enviada
                this.produtos.push(produtoSalvo || novoProduto);

                // Limpa o formulário na tela
                this.produtoForm.reset({ saldo: 0 }); // Devolve estado padrão ao validador do Saldo
                this.exibirMensagem('Produto cadastrado com sucesso!', 'sucesso');
                this.cdr.detectChanges();
            },
            error: (err) => {
                this.exibirMensagem(err.message, 'erro');
                this.cdr.detectChanges();
            }
        });
    }

    // Ajuda na exibição dos alertas visando expirá-los após um período
    exibirMensagem(texto: string, tipo: 'sucesso' | 'erro'): void {
        this.mensagem = { texto, tipo };
        setTimeout(() => {
            this.mensagem = null;
            this.cdr.detectChanges();
        }, 4000);
    }
}
