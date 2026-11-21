import { NgModel } from '@angular/forms';

export function validarNumero(
  event: Event,
  control: NgModel,
  tipo: 'precio' | 'cantidad'
): number {

  const input = event.target as HTMLInputElement;
  let valor = input.value;

  if (tipo === 'precio') {
    valor = valor.replace(/[^0-9.]/g, '');

    const partes = valor.split('.');

    if (partes.length > 1) {
      valor = partes[0] + '.' +
        partes.slice(1).join('').slice(0, 2);
    }
  } else {
    valor = valor.replace(/[^0-9]/g, '');
    valor = valor.slice(0, 6);
  }

  input.value = valor;
  control.control.setValue(valor, { emitEvent: false });

  return Number(valor);
}