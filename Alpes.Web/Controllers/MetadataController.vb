Option Strict On
Option Explicit On

Imports System.Web.Mvc

Public Class MetadataController
    Inherits Controller
    Function ListarTablas() As ActionResult
        Dim tablas = New List(Of Object) From {
            New With {.Value = "Usuario", .Text = "Usuario"},
            New With {.Value = "Rol", .Text = "Rol"},
            New With {.Value = "Orden_Venta", .Text = "Orden Venta"},
            New With {.Value = "Orden_Venta_Detalle", .Text = "Orden Venta Detalle"},
            New With {.Value = "Pago", .Text = "Pago"},
            New With {.Value = "Cuotas_Pago", .Text = "Cuotas Pago"},
            New With {.Value = "Factura", .Text = "Factura"},
            New With {.Value = "Factura_Detalle", .Text = "Factura_Detalle"},
            New With {.Value = "Devolucion", .Text = "Devolucion"},
            New With {.Value = "Historial_Compra", .Text = "Historial_Compra"},
            New With {.Value = "Resena_Comentario", .Text = "Resena_Comentario"},
            New With {.Value = "Regla_Envio_Gratis", .Text = "Regla_Envio_Gratis"},
            New With {.Value = "Tarifa_Envio", .Text = "Tarifa_Envio"},
            New With {.Value = "Promocion", .Text = "Promocion"},
            New With {.Value = "PromocionProducto", .Text = "PromocionProducto"},
            New With {.Value = "ReglaPromocion", .Text = "ReglaPromocion"},
            New With {.Value = "HistorialPromocion", .Text = "HistorialPromocion"},
            New With {.Value = "ExpedienteEmpleado", .Text = "ExpedienteEmpleado"},
            New With {.Value = "Evaluacion", .Text = "Evaluacion"},
            New With {.Value = "Nomina", .Text = "Nomina"},
            New With {.Value = "NominaDetalle", .Text = "NominaDetalle"},
            New With {.Value = "IncidenteLaboral", .Text = "IncidenteLaboral"}
        }

        Return Json(tablas, JsonRequestBehavior.AllowGet)
    End Function

    Function ObtenerTabla(ByVal nombre As String) As ActionResult
        Dim config As CrudTablaConfig = Nothing

        Select Case nombre.ToUpper()

            Case "USUARIO"
                config = New CrudTablaConfig With {
                    .TableName = "Usuario",
                    .Pk = "UsuId",
                    .Endpoint = "Usuario",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "UsuId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "Username", .Label = "Username", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "PasswordHash", .Label = "Password Hash", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Email", .Label = "Email", .Type = "text", .Required = False},
                        New CrudCampoConfig With {.Name = "Telefono", .Label = "Telefono", .Type = "text", .Required = False},
                        New CrudCampoConfig With {
                            .Name = "RolId",
                            .Label = "Rol",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Rol/Index",
                            .ValueField = "RolId",
                            .TextField = "RolNombre"
                        },
                        New CrudCampoConfig With {.Name = "CliId", .Label = "Cliente Id", .Type = "number", .Nullable = True},
                        New CrudCampoConfig With {.Name = "EmpId", .Label = "Empleado Id", .Type = "number", .Nullable = True},
                        New CrudCampoConfig With {.Name = "UltimoLoginAt", .Label = "Ultimo Login", .Type = "datetime-local", .Nullable = True},
                        New CrudCampoConfig With {.Name = "BloqueadoHasta", .Label = "Bloqueado Hasta", .Type = "datetime-local", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "ROL"
                config = New CrudTablaConfig With {
                    .TableName = "Rol",
                    .Pk = "RolId",
                    .Endpoint = "Rol",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "RolId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "RolNombre", .Label = "Nombre", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Descripcion", .Label = "Descripcion", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
                    }
                }


            Case "CUOTAS_PAGO"
                config = New CrudTablaConfig With {
        .TableName = "Cuotas_Pago",
        .Pk = "CuotaId",
        .Endpoint = "Cuotas_Pago",
        .Campos = New List(Of CrudCampoConfig) From {
            New CrudCampoConfig With {.Name = "CuotaId", .Label = "Id", .Type = "hidden"},
            New CrudCampoConfig With {.Name = "PagoId", .Label = "Pago Id", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "NumCuota", .Label = "Numero Cuota", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "MontoCuota", .Label = "Monto Cuota", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "FechaVencimiento", .Label = "Fecha Vencimiento", .Type = "date", .Nullable = True},
            New CrudCampoConfig With {.Name = "FechaPago", .Label = "Fecha Pago", .Type = "date", .Nullable = True},
            New CrudCampoConfig With {.Name = "CreatedAt", .Label = "Fecha Creacion", .Type = "datetime-local", .Required = True},
            New CrudCampoConfig With {.Name = "UpdatedAt", .Label = "Fecha Actualizacion", .Type = "datetime-local", .Nullable = True},
            New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
        }
    }
            Case "PAGO"
                config = New CrudTablaConfig With {
        .TableName = "Pago",
        .Pk = "PagoId",
        .Endpoint = "Pago",
        .Campos = New List(Of CrudCampoConfig) From {
            New CrudCampoConfig With {.Name = "PagoId", .Label = "Id", .Type = "hidden"},
            New CrudCampoConfig With {.Name = "OrdenVentaId", .Label = "Orden Venta Id", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "MetodoPagoId", .Label = "Metodo Pago Id", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "Monto", .Label = "Monto", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "EstadoPago", .Label = "Estado Pago", .Type = "text", .Nullable = True},
            New CrudCampoConfig With {.Name = "Referencia", .Label = "Referencia", .Type = "text", .Nullable = True},
            New CrudCampoConfig With {.Name = "PagoAt", .Label = "Fecha Pago", .Type = "datetime-local", .Required = True},
            New CrudCampoConfig With {.Name = "CreatedAt", .Label = "Fecha Creacion", .Type = "datetime-local", .Required = True},
            New CrudCampoConfig With {.Name = "UpdatedAt", .Label = "Fecha Actualizacion", .Type = "datetime-local", .Nullable = True},
            New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
        }
    }


            Case "ORDEN_VENTA"
                config = New CrudTablaConfig With {
        .TableName = "Orden_Venta",
        .Pk = "OrdenVentaId",
        .Endpoint = "Orden_Venta",
        .Campos = New List(Of CrudCampoConfig) From {
            New CrudCampoConfig With {.Name = "OrdenVentaId", .Label = "Id", .Type = "hidden"},
            New CrudCampoConfig With {.Name = "NumOrden", .Label = "Numero Orden", .Type = "text", .Required = True},
            New CrudCampoConfig With {.Name = "CliId", .Label = "Cliente Id", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "EstadoOrdenId", .Label = "Estado Orden Id", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "FechaOrden", .Label = "Fecha Orden", .Type = "datetime-local", .Required = True},
            New CrudCampoConfig With {.Name = "Subtotal", .Label = "Subtotal", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "Descuento", .Label = "Descuento", .Type = "number", .Required = True, .DefaultValue = "0"},
            New CrudCampoConfig With {.Name = "Impuesto", .Label = "Impuesto", .Type = "number", .Required = True, .DefaultValue = "0"},
            New CrudCampoConfig With {.Name = "Total", .Label = "Total", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "Moneda", .Label = "Moneda", .Type = "text", .Nullable = True},
            New CrudCampoConfig With {.Name = "DireccionEnvioSnapshot", .Label = "Direccion Envio", .Type = "text", .Nullable = True},
            New CrudCampoConfig With {.Name = "Observaciones", .Label = "Observaciones", .Type = "text", .Nullable = True},
            New CrudCampoConfig With {.Name = "CreatedAt", .Label = "Fecha Creacion", .Type = "datetime-local", .Required = True},
            New CrudCampoConfig With {.Name = "UpdatedAt", .Label = "Fecha Actualizacion", .Type = "datetime-local", .Nullable = True},
            New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
        }
    }


            Case "ORDEN_VENTA_DETALLE"
                config = New CrudTablaConfig With {
        .TableName = "Orden_Venta_Detalle",
        .Pk = "OrdenVentaDetId",
        .Endpoint = "Orden_Venta_Detalle",
        .Campos = New List(Of CrudCampoConfig) From {
            New CrudCampoConfig With {.Name = "OrdenVentaDetId", .Label = "Id", .Type = "hidden"},
            New CrudCampoConfig With {.Name = "OrdenVentaId", .Label = "Orden Venta Id", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "ProductoId", .Label = "Producto Id", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "Cantidad", .Label = "Cantidad", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "PrecioUnitarioSnapshot", .Label = "Precio Unitario", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "SubtotalLinea", .Label = "Subtotal Linea", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "CreatedAt", .Label = "Fecha Creacion", .Type = "datetime-local", .Required = True},
            New CrudCampoConfig With {.Name = "UpdatedAt", .Label = "Fecha Actualizacion", .Type = "datetime-local", .Nullable = True},
            New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
        }
    }
            Case "FACTURA"
                config = New CrudTablaConfig With {
        .TableName = "Factura",
        .Pk = "FacturaId",
        .Endpoint = "Factura",
        .Campos = New List(Of CrudCampoConfig) From {
            New CrudCampoConfig With {.Name = "FacturaId", .Label = "Id", .Type = "hidden"},
            New CrudCampoConfig With {.Name = "OrdenVentaId", .Label = "Orden Venta Id", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "NumFactura", .Label = "Numero Factura", .Type = "text", .Required = True},
            New CrudCampoConfig With {.Name = "FechaEmision", .Label = "Fecha Emision", .Type = "date", .Required = True},
            New CrudCampoConfig With {.Name = "NitFacturacion", .Label = "NIT Facturacion", .Type = "text", .Nullable = True},
            New CrudCampoConfig With {.Name = "DireccionFacturacionSnapshot", .Label = "Direccion Facturacion", .Type = "text", .Nullable = True},
            New CrudCampoConfig With {.Name = "TotalFacturaSnapshot", .Label = "Total Factura", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "CreatedAt", .Label = "Fecha Creacion", .Type = "datetime-local", .Nullable = True},
            New CrudCampoConfig With {.Name = "UpdatedAt", .Label = "Fecha Actualizacion", .Type = "datetime-local", .Nullable = True},
            New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
        }
    }
            Case "FACTURA_DETALLE"
                config = New CrudTablaConfig With {
                    .TableName = "Factura_Detalle",
                    .Pk = "FacturaDetId",
                    .Endpoint = "Factura_Detalle",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "FacturaDetId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "FacturaId", .Label = "Factura Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "ProductoId", .Label = "Producto Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Cantidad", .Label = "Cantidad", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "PrecioUnitarioSnapshot", .Label = "Precio Unitario", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "TotalLinea", .Label = "Total Linea", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "CreatedAt", .Label = "Fecha Creacion", .Type = "datetime-local", .Nullable = True},
                        New CrudCampoConfig With {.Name = "UpdatedAt", .Label = "Fecha Actualizacion", .Type = "datetime-local", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "DEVOLUCION"
                config = New CrudTablaConfig With {
                    .TableName = "Devolucion",
                    .Pk = "DevolucionId",
                    .Endpoint = "Devolucion",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "DevolucionId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "OrdenVentaId", .Label = "Orden Venta Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "CliId", .Label = "Cliente Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Motivo", .Label = "Motivo", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "EstadoDevolucion", .Label = "Estado Devolucion", .Type = "text", .Required = True, .DefaultValue = "SOLICITADA"},
                        New CrudCampoConfig With {.Name = "SolicitudAt", .Label = "Solicitud", .Type = "datetime-local", .Required = True},
                        New CrudCampoConfig With {.Name = "ResolucionAt", .Label = "Resolucion", .Type = "datetime-local", .Nullable = True},
                        New CrudCampoConfig With {.Name = "CreatedAt", .Label = "Fecha Creacion", .Type = "datetime-local", .Nullable = True},
                        New CrudCampoConfig With {.Name = "UpdatedAt", .Label = "Fecha Actualizacion", .Type = "datetime-local", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
                    }
                }
            Case "HISTORIAL_COMPRA"
                config = New CrudTablaConfig With {
        .TableName = "Historial_Compra",
        .Pk = "HistCompraId",
        .Endpoint = "HistorialCompra",
        .Campos = New List(Of CrudCampoConfig) From {
            New CrudCampoConfig With {.Name = "HistCompraId", .Label = "Id", .Type = "hidden"},
            New CrudCampoConfig With {.Name = "CliId", .Label = "Cliente Id", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "OrdenVentaId", .Label = "Orden Venta Id", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "CompraAt", .Label = "Fecha Compra", .Type = "datetime-local", .Required = True},
            New CrudCampoConfig With {.Name = "MontoTotalSnapshot", .Label = "Monto Total", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Required = False, .DefaultValue = "ACTIVO"}
        }
    }
            Case "RESENA_COMENTARIO"
                config = New CrudTablaConfig With {
                    .TableName = "Resena_Comentario",
                    .Pk = "ResenaId",
                    .Endpoint = "ResenaComentario",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "ResenaId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "CliId", .Label = "Cliente Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "ProductoId", .Label = "Producto Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Calificacion", .Label = "Calificacion", .Type = "number", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Comentario", .Label = "Comentario", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "ResenaAt", .Label = "Fecha Resena", .Type = "datetime-local", .Required = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Required = False, .DefaultValue = "ACTIVO"}
                    }
                }
            Case "REGLA_ENVIO_GRATIS"
                config = New CrudTablaConfig With {
        .TableName = "Regla_Envio_Gratis",
        .Pk = "ReglaEnvioGratisId",
        .Endpoint = "ReglaEnvioGratis",
        .Campos = New List(Of CrudCampoConfig) From {
            New CrudCampoConfig With {.Name = "ReglaEnvioGratisId", .Label = "Id", .Type = "hidden"},
            New CrudCampoConfig With {.Name = "ZonaEnvioId", .Label = "Zona Envio Id", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "MontoMinimo", .Label = "Monto Minimo", .Type = "number", .Nullable = True},
            New CrudCampoConfig With {.Name = "PesoMaxKg", .Label = "Peso Max Kg", .Type = "number", .Nullable = True},
            New CrudCampoConfig With {.Name = "VigenciaInicio", .Label = "Vigencia Inicio", .Type = "date", .Nullable = True},
            New CrudCampoConfig With {.Name = "VigenciaFin", .Label = "Vigencia Fin", .Type = "date", .Nullable = True},
            New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Required = False, .DefaultValue = "ACTIVO"}
        }
    }
            Case "TARIFA_ENVIO"
                config = New CrudTablaConfig With {
        .TableName = "Tarifa_Envio",
        .Pk = "TarifaEnvioId",
        .Endpoint = "TarifaEnvio",
        .Campos = New List(Of CrudCampoConfig) From {
            New CrudCampoConfig With {.Name = "TarifaEnvioId", .Label = "Id", .Type = "hidden"},
            New CrudCampoConfig With {.Name = "ZonaEnvioId", .Label = "Zona Envio Id", .Type = "text", .Required = True},
            New CrudCampoConfig With {.Name = "TipoEntregaId", .Label = "Tipo Entrega Id", .Type = "text", .Required = True},
            New CrudCampoConfig With {.Name = "PesoDesdeKg", .Label = "Peso Desde Kg", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "PesoHastaKg", .Label = "Peso Hasta Kg", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "Costo", .Label = "Costo", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Required = False, .DefaultValue = "ACTIVO"}
        }
    }
            Case "PROMOCION"
                config = New CrudTablaConfig With {
        .TableName = "Promocion",
        .Pk = "PromocionId",
        .Endpoint = "Promocion",
        .Campos = New List(Of CrudCampoConfig) From {
            New CrudCampoConfig With {.Name = "PromocionId", .Label = "Id", .Type = "hidden"},
            New CrudCampoConfig With {.Name = "TipoPromocionId", .Label = "Tipo Promocion Id", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "Nombre", .Label = "Nombre", .Type = "text", .Required = True},
            New CrudCampoConfig With {.Name = "Descripcion", .Label = "Descripcion", .Type = "text", .Nullable = True},
            New CrudCampoConfig With {.Name = "VigenciaInicio", .Label = "Vigencia Inicio", .Type = "datetime-local", .Required = True},
            New CrudCampoConfig With {.Name = "VigenciaFin", .Label = "Vigencia Fin", .Type = "datetime-local", .Required = True},
            New CrudCampoConfig With {.Name = "Prioridad", .Label = "Prioridad", .Type = "number", .Required = True, .DefaultValue = "0"},
            New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Nullable = True, .DefaultValue = "ACTIVO"}
        }
    }
            Case "PROMOCIONPRODUCTO"
                config = New CrudTablaConfig With {
                    .TableName = "PromocionProducto",
                    .Pk = "PromocionProductoId",
                    .Endpoint = "PromocionProducto",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "PromocionProductoId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "PromocionId", .Label = "Promocion Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "ProductoId", .Label = "Producto Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "LimiteUnidades", .Label = "Limite Unidades", .Type = "number"},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .DefaultValue = "ACTIVO"}
                    }
                }
            Case "REGLAPROMOCION"
                config = New CrudTablaConfig With {
                    .TableName = "ReglaPromocion",
                    .Pk = "ReglaPromocionId",
                    .Endpoint = "ReglaPromocion",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "ReglaPromocionId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "PromocionId", .Label = "Promocion Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "MinCompra", .Label = "Min Compra", .Type = "number"},
                        New CrudCampoConfig With {.Name = "MinItems", .Label = "Min Items", .Type = "number"},
                        New CrudCampoConfig With {.Name = "AplicaTipoProducto", .Label = "Aplica Tipo Producto", .Type = "text"},
                        New CrudCampoConfig With {.Name = "TopeDescuento", .Label = "Tope Descuento", .Type = "number"},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .DefaultValue = "ACTIVO"}
                    }
                }
            Case "HISTORIALPROMOCION"
                config = New CrudTablaConfig With {
                    .TableName = "HistorialPromocion",
                    .Pk = "HistorialPromocionId",
                    .Endpoint = "HistorialPromocion",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "HistorialPromocionId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "OrdenVentaId", .Label = "Orden Venta Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "PromocionId", .Label = "Promocion Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "MontoDescuentoSnapshot", .Label = "Monto Descuento Snapshot", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .DefaultValue = "ACTIVO"}
                    }
                }
            Case "EXPEDIENTEEMPLEADO"
                config = New CrudTablaConfig With {
                    .TableName = "ExpedienteEmpleado",
                    .Pk = "ExpedienteEmpleadoId",
                    .Endpoint = "ExpedienteEmpleado",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "ExpedienteEmpleadoId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "EmpId", .Label = "Empleado Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "TipoDocumento", .Label = "Tipo Documento", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "UrlDocumento", .Label = "Url Documento", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "FechaDocumento", .Label = "Fecha Documento", .Type = "datetime-local"},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .DefaultValue = "ACTIVO"}
                    }
                }
            Case "EVALUACION"
                config = New CrudTablaConfig With {
                    .TableName = "Evaluacion",
                    .Pk = "EvaluacionId",
                    .Endpoint = "Evaluacion",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "EvaluacionId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "EmpId", .Label = "Empleado Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "EvaluadorEmpId", .Label = "Evaluador Empleado Id", .Type = "number"},
                        New CrudCampoConfig With {.Name = "FechaEval", .Label = "Fecha Evaluacion", .Type = "datetime-local", .Required = True},
                        New CrudCampoConfig With {.Name = "Puntuacion", .Label = "Puntuacion", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Comentarios", .Label = "Comentarios", .Type = "text"},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .DefaultValue = "ACTIVO"}
                    }
                }
            Case "NOMINA"
                config = New CrudTablaConfig With {
                    .TableName = "Nomina",
                    .Pk = "NominaId",
                    .Endpoint = "Nomina",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "NominaId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "EmpId", .Label = "Empleado Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "PeriodoInicio", .Label = "Periodo Inicio", .Type = "datetime-local", .Required = True},
                        New CrudCampoConfig With {.Name = "PeriodoFin", .Label = "Periodo Fin", .Type = "datetime-local", .Required = True},
                        New CrudCampoConfig With {.Name = "MontoBruto", .Label = "Monto Bruto", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "MontoNeto", .Label = "Monto Neto", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "FechaPago", .Label = "Fecha Pago", .Type = "datetime-local", .Required = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .DefaultValue = "PENDIENTE"}
                    }
                }
            Case "NOMINADETALLE"
                config = New CrudTablaConfig With {
                    .TableName = "NominaDetalle",
                    .Pk = "NominaDetalleId",
                    .Endpoint = "NominaDetalle",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "NominaDetalleId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "NominaId", .Label = "Nomina Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Tipo", .Label = "Tipo", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Concepto", .Label = "Concepto", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Monto", .Label = "Monto", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .DefaultValue = "ACTIVO"}
                    }
                }
            Case "INCIDENTELABORAL"
                config = New CrudTablaConfig With {
                    .TableName = "IncidenteLaboral",
                    .Pk = "IncidenteId",
                    .Endpoint = "IncidenteLaboral",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "IncidenteId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "EmpId", .Label = "Empleado Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "FechaIncidente", .Label = "Fecha Incidente", .Type = "datetime-local", .Required = True},
                        New CrudCampoConfig With {.Name = "Descripcion", .Label = "Descripcion", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Gravedad", .Label = "Gravedad", .Type = "text"},
                        New CrudCampoConfig With {.Name = "AccionesTomadas", .Label = "Acciones Tomadas", .Type = "text"},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .DefaultValue = "ACTIVO"}
                    }
                }
        End Select

        If config Is Nothing Then
            Dim respuesta = New With {
                .success = False,
                .message = "No existe metadata para la tabla solicitada."
            }

            Return Json(respuesta, JsonRequestBehavior.AllowGet)
        End If

        Return Json(config, JsonRequestBehavior.AllowGet)
    End Function

End Class