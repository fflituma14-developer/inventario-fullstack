export interface LoteProducto {
  idLote: number;
  idProducto: number;
  idProveedor: number;
  numeroLote: string;
  precioCompra: number;
  cantidadStock: number;
  fechaIngreso: string;
  estado: string;
}