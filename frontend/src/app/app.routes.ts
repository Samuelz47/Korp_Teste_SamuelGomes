import { Routes } from '@angular/router';
import { ProdutosComponent } from './features/produtos/produtos.component';
import { FaturamentoComponent } from './features/faturamento/faturamento.component';
import { NotasFiscais } from './features/notas-fiscais/notas-fiscais';

export const routes: Routes = [
    { path: 'produtos', component: ProdutosComponent },
    { path: 'faturamento', component: FaturamentoComponent },
    { path: 'notas-fiscais', component: NotasFiscais },
    { path: '', redirectTo: 'produtos', pathMatch: 'full' }
];
