import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Produto } from '../models/produto.model';

@Injectable({
    providedIn: 'root'
})
export class EstoqueService {
    private http = inject(HttpClient);
    private apiUrl = `${environment.estoqueApi}/produtos`;

    obterProdutos(): Observable<Produto[]> {
        return this.http.get<Produto[]>(this.apiUrl).pipe(
            catchError(this.handleError)
        );
    }

    cadastrarProduto(produto: Produto): Observable<Produto> {
        return this.http.post<Produto>(this.apiUrl, produto).pipe(
            catchError(this.handleError)
        );
    }

    private handleError(error: HttpErrorResponse) {
        let errorMessage = 'Ocorreu um erro desconhecido.';
        if (error.error instanceof ErrorEvent) {
            errorMessage = `Erro: ${error.error.message}`;
        } else {
            errorMessage = `Código do erro: ${error.status}, Mensagem: ${error.message}`;
        }
        console.error('Erro no EstoqueService:', errorMessage);
        return throwError(() => new Error(errorMessage));
    }
}
