# CRUD APIs para entidades

El proyecto ahora expone endpoints JSON para las entidades que ya estaban implementadas en las capas `Servicios` y `Datos`.

## Patrón de endpoints

Cada entidad expone:

- `GET /api/<recurso>`: listar.
- `GET /api/<recurso>/{id}`: obtener por id.
- `GET /api/<recurso>/buscar?criterio=<criterio>&valor=<valor>`: buscar.
- `POST /api/<recurso>`: crear.
- `PUT /api/<recurso>/{id}`: actualizar.
- `DELETE /api/<recurso>/{id}`: eliminar.

## Recursos disponibles

- `/api/campanas-marketing`
- `/api/clientes`
- `/api/cupones`
- `/api/estados-envio`
- `/api/politicas-envio`
- `/api/promociones`
- `/api/promociones-producto`
- `/api/reglas-envio-gratis`
- `/api/reglas-promocion`
- `/api/tarifas-envio`
- `/api/tipos-entrega`
- `/api/tipos-promocion`
- `/api/zonas-envio`

## Alcance real

Estas APIs cubren las entidades que hoy tienen una capa `Servicios/*Servicio.vb` para este módulo CRUD (`CampanaMarketing`, `Cliente`, `Cupon`, `EstadoEnvio`, `PoliticaEnvio`, `Promocion`, `PromocionProducto`, `ReglaEnvioGratis`, `ReglaPromocion`, `TarifaEnvio`, `TipoEntrega`, `TipoPromocion` y `ZonaEnvio`).

No cubren automáticamente todas las clases de `Datos/*.vb` ni todas las tablas del repositorio; para eso haría falta exponer también las entidades que todavía no tienen un servicio/controlador equivalente en esta solución.

## Formato de respuesta

### Respuesta exitosa

```json
{
  "success": true,
  "data": []
}
```

### Respuesta de creación

```json
{
  "success": true,
  "id": 123
}
```

### Error

```json
{
  "success": false,
  "message": "Mensaje de error",
  "details": null
}
```

## Notas

- Las operaciones siguen reutilizando los procedimientos almacenados ya definidos en la capa de datos.
- Los `DateTime` se serializan en formato ISO 8601.
- Para `PUT`, el id de la ruta prevalece sobre el id enviado en el cuerpo.
