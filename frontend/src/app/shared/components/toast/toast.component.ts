import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastService } from '../../../core/services/toast.service';

@Component({
    selector: 'app-toast',
    standalone: true,
    imports: [CommonModule],
    template: `
    <div class="fixed top-5 right-5 z-[9999] flex flex-col gap-3 pointer-events-none">
      <div *ngFor="let toast of toastService.toasts()" 
           class="px-5 py-4 rounded-md shadow-2xl flex items-center justify-between min-w-[320px] max-w-sm transition-all transform pointer-events-auto"
           [ngClass]="{
             'bg-red-600 text-white': toast.type === 'error',
             'bg-green-600 text-white': toast.type === 'success',
             'bg-yellow-500 text-white': toast.type === 'warning'
           }">
        <span class="font-medium text-sm leading-tight mr-4">{{ toast.message }}</span>
        <button (click)="toastService.remove(toast.id)" class="text-white hover:text-gray-200 focus:outline-none shrink-0 cursor-pointer p-1">
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
          </svg>
        </button>
      </div>
    </div>
  `
})
export class ToastComponent {
    toastService = inject(ToastService);
}
