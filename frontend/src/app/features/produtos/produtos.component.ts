import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { EstoqueService } from '../../core/services/estoque.service';
import { Produto } from '../../core/models/produto.model';
import { ToastService } from '../../core/services/toast.service';

@Component({
    selector: 'app-produtos',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule],
    templateUrl: './produtos.component.html'
})
export class ProdutosComponent implements OnInit {
    private estoqueService = inject(EstoqueService);
    private fb = inject(FormBuilder);
    private cdr = inject(ChangeDetectorRef);
    private toastService = inject(ToastService);

    produtos: Produto[] = [];
    produtoForm: FormGroup;
    isLoading = false;

    constructor() {
        this.produtoForm = this.fb.group({
            codigo: ['', Validators.required],
            descricao: ['', Validators.required],
            saldo: [0, [Validators.required, Validators.pattern('^[0-9]+(\\.[0-9]{1,2})?$')]]
        });
    }

    ngOnInit(): void {
        this.carregarProdutos();
    }
    carregarProdutos(): void {
        this.estoqueService.obterProdutos().subscribe({
            next: (dados) => {
                this.produtos = dados;
                this.cdr.detectChanges();
            },
            error: (_) => {
                this.cdr.detectChanges();
            }
        });
    }

    onSubmit(): void {
        if (this.produtoForm.invalid) {
            Object.keys(this.produtoForm.controls).forEach(key => {
                this.produtoForm.get(key)?.markAsTouched();
            });
            return;
        }

        const novoProduto: Produto = this.produtoForm.value;
        this.isLoading = true;

        this.estoqueService.cadastrarProduto(novoProduto).subscribe({
            next: (produtoSalvo) => {
                this.produtos.push(produtoSalvo || novoProduto);
                this.produtoForm.reset({ saldo: 0 });
                this.toastService.show('Produto cadastrado com sucesso!', 'success');
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
