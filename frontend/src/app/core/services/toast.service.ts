import { Injectable, signal } from '@angular/core';

export interface Toast {
    message: string;
    type: 'success' | 'error' | 'warning';
    id: number;
}

@Injectable({ providedIn: 'root' })
export class ToastService {
    toasts = signal<Toast[]>([]);
    private idCounter = 0;

    show(message: string, type: 'success' | 'error' | 'warning' = 'error') {
        const id = this.idCounter++;
        this.toasts.update(t => [...t, { message, type, id }]);

        // Auto remover após 5 segundos
        setTimeout(() => this.remove(id), 5000);
    }

    remove(id: number) {
        this.toasts.update(t => t.filter(toast => toast.id !== id));
    }
}
