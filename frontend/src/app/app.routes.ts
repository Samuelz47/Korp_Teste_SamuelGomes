import { Routes } from '@angular/router';
import { ProdutosComponent } from './features/produtos/produtos.component';
import { FaturamentoComponent } from './features/faturamento/faturamento.component';

export const routes: Routes = [
    { path: 'produtos', component: ProdutosComponent },
    { path: 'faturamento', component: FaturamentoComponent },
    { path: '', redirectTo: 'produtos', pathMatch: 'full' }
];
