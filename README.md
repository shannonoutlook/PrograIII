Proyecto 1 Programación 3

Shannon Alejandra Estrada Hernández 0900-20-1184
Pablo José Gudiel Sandoval 0900-18-1477

El presente proyecto desarrolla una API con EndPoints para la gestión de tarjetas de crédito por medio de estructura de datos.

1.Carga inicial
La carga inicial de los datos de clientes de tarjetas de crédito se realiza en el endpoint de carga inicial de datos, el HttpPost admite un formato de lista Json del siguiente esquema:
[
  {
    "numeroTarjeta": "string",
    "nombreTarjeta": "string",
    "saldo": 0,
    "limiteCredito": 0,
    "fechaCorte": "string",
    "fechaPago": "string",
    "puntos": 0,
    "pin": 0,
    "estatus": true
  }
]

Se ingresa la lista de clientes y se ejecuta el EndPoint, si el formato cumple correctamente debe mostrar el mensaje: X tarjetas de crédito se agregaron correctamente.

2. Consulta de saldo
Debe ingresar el parámetro del número de tarjea para que el servicio devuelva el saldo que contiene la tarjeta del cliente.

3. Realización de pagos
El EndPoint solicita como entrada el número detrjeta deseado así como el montó que se abonará a la tarjeta, muestra el resultado generado y debita del saldo de la tarjeta la cantidad indicada con el siguiente formato:
{
  "creditCardNumber": 0,
  "amount": 0
}

4. Generación de estados de cuenta
El endpoint solicita el número de tarjeta y utiliza arboles binarios de busqueda para encontrar el tarjetahabiente y devolver su estado de cuenta y se observará con el siguiente formato:
{
  "numeroTarjeta": 1234567890123456,
  "nombreTarjeta": "Visa Platinum",
  "saldo": 3500,
  "limiteCredito": 50000,
  "fechaCorte": "2024-05-15",
  "fechaPago": "2024-05-30",
  "puntos": 1200,
  "estadoBloqueo": false,
  "movimientos": [
    {
      "creditCardNumber": 1234567890123456,
      "amount": 500
    },
    {
      "creditCardNumber": 1234567890123456,
      "amount": 1000
    }
  ]
}

5. Consulta de movimientos
Se solicita el parámetro de número de tarjeta para retornar los movimientos realizados a la tarjeta de credito como los pagos realizados:
Ejemplo de movimientos:
[
  {
    "creditCardNumber": 3456789012345678,
    "amount": 700
  },
  {
    "creditCardNumber": 2345678901234567,
    "amount": 100
  },
  {
    "creditCardNumber": 2345678901234567,
    "amount": 300
  },
  {
    "creditCardNumber": 2345678901234567,
    "amount": 100
  },
  {
    "creditCardNumber": 1234567890123456,
    "amount": 500
  },
  {
    "creditCardNumber": 1234567890123456,
    "amount": 1000
  }
]

6. Notificaciones
Se solicita el mensaje de la notificación que se desea enviar, al ejecutar el endpoint envía la notificación con la inforamción del mensaje, la fecha y la hora:
Ejemplo de notificaciones:
[
  {
    "message": "Pago de 1000 procesado a la tarjeta 1234567890123456. Saldo actual: 4000.",
    "timestamp": "0001-01-01T00:00:00"
  },
  {
    "message": "Pago de 500 procesado a la tarjeta 1234567890123456. Saldo actual: 3500.",
    "timestamp": "0001-01-01T00:00:00"
  },
  {
    "message": "Pago de 100 procesado a la tarjeta 2345678901234567. Saldo actual: 1400.",
    "timestamp": "0001-01-01T00:00:00"
  },
  {
    "message": "Pago de 300 procesado a la tarjeta 2345678901234567. Saldo actual: 1100.",
    "timestamp": "0001-01-01T00:00:00"
  },
  {
    "message": "Pago de 100 procesado a la tarjeta 2345678901234567. Saldo actual: 1000.",
    "timestamp": "0001-01-01T00:00:00"
  },
  {
    "message": "Pago de 700 procesado a la tarjeta 3456789012345678. Saldo actual: 50.",
    "timestamp": "0001-01-01T00:00:00"
  },
  {
    "message": "PIN actualizado para la tarjeta 1234567890123456.",
    "timestamp": "0001-01-01T00:00:00"
  },
  {
    "message": "PIN actualizado para la tarjeta 3456789012345678.",
    "timestamp": "0001-01-01T00:00:00"
  },
  {
    "message": "La tarjeta 3456789012345678 ha sido bloqueada temporalmente.",
    "timestamp": "0001-01-01T00:00:00"
  },
  {
    "message": "Límite de crédito aumentado a 100000 para la tarjeta 2345678901234567.",
    "timestamp": "0001-01-01T00:00:00"
  },
  {
    "message": "Límite de crédito aumentado a 50000 para la tarjeta 1234567890123456.",
    "timestamp": "0001-01-01T00:00:00"
  }
]

7. Cambio de Pin
Se solicita el número de la tarjeta que se desea reemplazar el pin, y el nuevo pin, el servicio actualiza la información en memoria del registro del tarjetahabiente con el siguiente formato:
{
  "creditCardNumber": 0,
  "oldPin": "string",
  "newPin": "string"
}

8. Bloqueo de Tarjetas
El Endpoint bloquea tarjetas solicitando el parámetro del número de tarjeta.
Si la tarjeta existe se utiliza un árbol binario de búsqueda para encontrar la tarjeta y bloequearla:
Tarjeta bloqueada con éxito

9. Solicitudes de aumento de crédito
Se ingresa el parámetro de cantidad de límite de crédito, el servicio devuelve un mensaje de correcto si el tarjetahabiente existe y realiza las modificaciones con el siguiente formato:
{
  "creditCardNumber": 0,
  "newCreditLimit": 0
}
