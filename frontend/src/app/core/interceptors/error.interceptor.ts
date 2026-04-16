import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { ToastService } from '../services/toast.service';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
    const toastService = inject(ToastService);

    return next(req).pipe(
        catchError((error: HttpErrorResponse) => {
            let errorMessage = 'Ocorreu um erro inesperado.';

            if (error.status === 0 || error.status === 500) {
                errorMessage = 'Serviço indisponível no momento. Tente novamente mais tarde.';
            } else if (error.status === 400 || error.status === 422) {
                if (error.error) {
                    if (typeof error.error === 'string') {
                        errorMessage = error.error;
                    } else if (error.error.Erro) { // C# Anonymous object format { Erro: "..." }
                        errorMessage = error.error.Erro;
                    } else if (error.error.erro) {
                        errorMessage = error.error.erro;
                    } else if (error.error.detail) {
                        errorMessage = error.error.detail;
                    } else if (error.error.message) {
                        errorMessage = error.error.message;
                    } else {
                        errorMessage = 'Requisição com dados inválidos.';
                    }
                }
            } else {
                errorMessage = `Erro ${error.status}: ${error.message}`;
            }

            toastService.show(errorMessage, 'error');

            return throwError(() => error);
        })
    );
};
