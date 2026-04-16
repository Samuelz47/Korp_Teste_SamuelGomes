import { NotaFiscalItemDTO } from './nota-fiscal-item-dto.model';

export interface NotaFiscal {
    id: string;
    numeroSequencial: number;
    status: number; // 0 para Fechada, 1 para Aberta
    itens: NotaFiscalItemDTO[];
}
