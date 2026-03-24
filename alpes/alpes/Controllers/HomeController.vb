Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Private Shared ReadOnly _entidadesCrud As String() = {
        "Rol", "Permiso", "Rol_Permiso", "Departamento", "Cargo", "Rol_Empleado", "Empleado", "Cliente", "Unidad_Medida", "Categoria", "Producto", "Inventario_Producto", "Movimiento_Inventario", "Precio_Historico", "Estado_Produccion", "Materia_Prima", "Inventario_Materia_Prima", "Movimiento_Materia_Prima", "Plan_Produccion", "Orden_Produccion", "Lista_Materiales", "Lista_Materiales_Detalle", "Herramienta", "Mantenimiento_Herramienta", "Orden_Produccion_Tarea", "Consumo_Materia_Prima", "Produccion_Resultados", "Proveedor", "Estado_Orden_Compra", "Condicion_Pago", "Orden_Compra", "Orden_Compra_Detalle", "Recepcion_Material", "Abastecimiento", "Cuenta_Pagar_Proveedor", "Pago_Proveedor", "Contrato_Proveedor", "Expediente_Proveedor", "Historico_Precio_Materia_Prima_Proveedor", "Control_Calidad", "Estado_Orden", "Metodo_Pago", "Orden_Venta", "Orden_Venta_Detalle", "Pago", "Cuotas_Pago", "Factura", "Factura_Detalle", "Devolucion", "Usuario", "Sesion", "Historial_Cambio_Contrasena", "Historial_Compra", "Preferencia_Cliente", "Carrito", "Carrito_Detalle", "Lista_Deseos", "Resena_Comentario", "FAQ", "Estado_Envio", "Tipo_Entrega", "Zona_Envio", "Politica_Envio", "Regla_Envio_Gratis", "Tarifa_Envio", "Vehiculo", "Ruta_Entrega", "Envio", "Seguimiento_Envio", "Tipo_Promocion", "Promocion", "Promocion_Producto", "Regla_Promocion", "Campana_Marketing", "Cupon", "Historial_Promocion", "Historial_Laboral", "Expediente_Empleado", "Evaluacion", "Nomina", "Nomina_Detalle", "Incidente_Laboral"
    }

    Function Index() As ActionResult
        Return View()
    End Function

    Function About() As ActionResult
        ViewData("Message") = "Your application description page."

        Return View()
    End Function

    Function Contact() As ActionResult
        ViewData("Message") = "Your contact page."

        Return View()
    End Function

    Function Cruds() As ActionResult
        ViewBag.Entidades = _entidadesCrud
        Return View()
    End Function
End Class
