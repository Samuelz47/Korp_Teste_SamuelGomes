import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FaturamentoService } from '../../core/services/faturamento.service';
import { NotaFiscal } from '../../core/models/nota-fiscal.model';
import { ToastService } from '../../core/services/toast.service';

@Component({
  selector: 'app-notas-fiscais',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './notas-fiscais.html',
  styleUrl: './notas-fiscais.css'
})
export class NotasFiscais implements OnInit {
  private faturamentoService = inject(FaturamentoService);
  private cdr = inject(ChangeDetectorRef);
  private toastService = inject(ToastService);

  notasFiscais: NotaFiscal[] = [];
  processingId: string | null = null; // para mostrar o spinner no botão específico

  ngOnInit(): void {
    this.carregarNotas();
  }

  carregarNotas(): void {
    this.faturamentoService.obterTodas().subscribe({
      next: (dados) => {
        this.notasFiscais = dados;
        this.cdr.detectChanges();
      },
      error: (_) => {
        this.cdr.detectChanges();
      }
    });
  }

  imprimirNota(id: string): void {
    this.processingId = id;
    this.cdr.detectChanges();

    this.faturamentoService.fecharNota(id).subscribe({
      next: () => {
        this.toastService.show('Nota Fiscal fechada e impressa com sucesso!', 'success');
        // Atualiza a nota na lista local para refletir a mudança imediatamente
        const nota = this.notasFiscais.find(n => n.id === id);
        if (nota) {
          nota.status = 0; // 0 = Fechada
        }
        this.processingId = null;
        this.cdr.detectChanges();
      },
      error: (_) => {
        this.processingId = null;
        this.cdr.detectChanges();
      }
    });
  }
}
