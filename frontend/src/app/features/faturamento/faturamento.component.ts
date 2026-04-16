import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormArray, ReactiveFormsModule, Validators } from '@angular/forms';
import { FaturamentoService } from '../../core/services/faturamento.service';
import { NotaFiscalForRegistrationDTO } from '../../core/models/nota-fiscal-for-registration-dto.model';
import { NotaFiscalItemDTO } from '../../core/models/nota-fiscal-item-dto.model';
import { ToastService } from '../../core/services/toast.service';

@Component({
    selector: 'app-faturamento',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule],
    templateUrl: './faturamento.component.html'
})
export class FaturamentoComponent implements OnInit {

    private faturamentoService = inject(FaturamentoService);
    private fb = inject(FormBuilder);
    private cdr = inject(ChangeDetectorRef);
    private toastService = inject(ToastService);

    faturamentoForm!: FormGroup;
    statusVisual = 'Aberta';
    isLoading = false;

    ngOnInit(): void {
        this.faturamentoForm = this.fb.group({
            itens: this.fb.array([])
        });
        this.adicionarItem();
    }

    get itensFormArray(): FormArray {
        return this.faturamentoForm.get('itens') as FormArray;
    }
    novoItemFormGroup(): FormGroup {
        return this.fb.group({
            produtoCodigo: ['', Validators.required],
            quantidade: [1, [Validators.required, Validators.min(1)]]
        });
    }

    adicionarItem(): void {
        this.itensFormArray.push(this.novoItemFormGroup());
    }
    removerItem(index: number): void {
        if (this.itensFormArray.length > 1) {
            this.itensFormArray.removeAt(index);
        }
    }

    onSubmit(): void {
        if (this.faturamentoForm.invalid) {
            this.faturamentoForm.markAllAsTouched();
            return;
        }

        if (this.itensFormArray.length === 0) {
            return;
        }
        const dto: NotaFiscalForRegistrationDTO = {
            itens: this.faturamentoForm.value.itens as NotaFiscalItemDTO[]
        };

        this.isLoading = true;

        this.faturamentoService.gerarNotaFiscal(dto).subscribe({
            next: () => {
                this.itensFormArray.clear();
                this.faturamentoForm.reset();
                this.adicionarItem();
                this.toastService.show('Nota fiscal gerada com sucesso!', 'success');
                this.isLoading = false;
                this.cdr.detectChanges();
            },
            error: (_) => {
                this.isLoading = false;
                this.cdr.detectChanges();
            }
        });
    }
}
