import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { NotaFiscalForRegistrationDTO } from '../models/nota-fiscal-for-registration-dto.model';

@Injectable({
    providedIn: 'root'
})
export class FaturamentoService {
    private http = inject(HttpClient);
    private apiUrl = `${environment.faturamentoApi}/NotasFiscais`;

    gerarNotaFiscal(notaFiscal: NotaFiscalForRegistrationDTO): Observable<any> {
        return this.http.post<any>(this.apiUrl, notaFiscal).pipe(
            catchError(this.handleError)
        );
    }

    private handleError(error: HttpErrorResponse) {
        let errorMessage = 'Ocorreu um erro desconhecido ao comunicar com o Faturamento.';
        if (error.error instanceof ErrorEvent) {
            errorMessage = `Erro (cliente): ${error.error.message}`;
        } else {
            errorMessage = `Erro na API (código ${error.status}): ${error.message}`;
        }
        console.error('Erro no FaturamentoService:', errorMessage);
        return throwError(() => new Error(errorMessage));
    }
}
