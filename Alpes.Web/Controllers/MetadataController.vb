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
            New With {.Value = "Factura", .Text = "Factura"}
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
                    .Pk = "cuota_id",
                    .Endpoint = "Cuotas_Pago",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "cuota_id", .Label = "ID", .Type = "hidden"},
                        New CrudCampoConfig With {
                            .Name = "pago_id",
                            .Label = "Pago",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Pago/Index",
                            .ValueField = "pago_id",
                            .TextField = "referencia"
                        },
                        New CrudCampoConfig With {.Name = "num_cuota", .Label = "Número Cuota", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "monto_cuota", .Label = "Monto Cuota", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "fecha_vencimiento", .Label = "Fecha Vencimiento", .Type = "date", .Nullable = True},
                        New CrudCampoConfig With {.Name = "fecha_pago", .Label = "Fecha Pago", .Type = "date", .Nullable = True},
                        New CrudCampoConfig With {.Name = "estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
                    }
                }
            Case "PAGO"
                config = New CrudTablaConfig With {
                    .TableName = "Pago",
                    .Pk = "pago_id",
                    .Endpoint = "Pago",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "pago_id", .Label = "ID", .Type = "hidden"},
                        New CrudCampoConfig With {
                            .Name = "orden_venta_id",
                            .Label = "Orden Venta",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Orden_Venta/Index",
                            .ValueField = "orden_venta_id",
                            .TextField = "num_orden"
                        },
                        New CrudCampoConfig With {
                            .Name = "metodo_pago_id",
                            .Label = "Método Pago",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Metodo_Pago/Index",
                            .ValueField = "metodo_pago_id",
                            .TextField = "nombre"
                        },
                        New CrudCampoConfig With {.Name = "monto", .Label = "Monto", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "estado_pago", .Label = "Estado Pago", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "referencia", .Label = "Referencia", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "pago_at", .Label = "Fecha Pago", .Type = "datetime-local", .Required = True},
                        New CrudCampoConfig With {.Name = "estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
                    }
                }


            Case "ORDEN_VENTA"
                config = New CrudTablaConfig With {
                    .TableName = "Orden_Venta",
                    .Pk = "orden_venta_id",
                    .Endpoint = "Orden_Venta",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "orden_venta_id", .Label = "ID", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "num_orden", .Label = "Número Orden", .Type = "text", .Required = True},
                        New CrudCampoConfig With {
                            .Name = "cli_id",
                            .Label = "Cliente",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Cliente/Index",
                            .ValueField = "cli_id",
                            .TextField = "nombres"
                        },
                        New CrudCampoConfig With {
                            .Name = "estado_orden_id",
                            .Label = "Estado Orden",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Estado_Orden/Index",
                            .ValueField = "estado_orden_id",
                            .TextField = "descripcion"
                        },
                        New CrudCampoConfig With {.Name = "fecha_orden", .Label = "Fecha Orden", .Type = "datetime-local", .Required = True},
                        New CrudCampoConfig With {.Name = "subtotal", .Label = "Subtotal", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "descuento", .Label = "Descuento", .Type = "number", .Nullable = True},
                        New CrudCampoConfig With {.Name = "impuesto", .Label = "Impuesto", .Type = "number", .Nullable = True},
                        New CrudCampoConfig With {.Name = "total", .Label = "Total", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "moneda", .Label = "Moneda", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "direccion_envio_snapshot", .Label = "Dirección", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "observaciones", .Label = "Observaciones", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
                    }
                }


            Case "ORDEN_VENTA_DETALLE"
                config = New CrudTablaConfig With {
                    .TableName = "Orden_Venta_Detalle",
                    .Pk = "orden_venta_det_id",
                    .Endpoint = "Orden_Venta_Detalle",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "orden_venta_det_id", .Label = "ID", .Type = "hidden"},
                        New CrudCampoConfig With {
                            .Name = "orden_venta_id",
                            .Label = "Orden Venta",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Orden_Venta/Index",
                            .ValueField = "orden_venta_id",
                            .TextField = "num_orden"
                        },
                        New CrudCampoConfig With {
                            .Name = "producto_id",
                            .Label = "Producto",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Producto/Index",
                            .ValueField = "producto_id",
                            .TextField = "nombre"
                        },
                        New CrudCampoConfig With {.Name = "cantidad", .Label = "Cantidad", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "precio_unitario_snapshot", .Label = "Precio Unitario", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "subtotal_linea", .Label = "Subtotal Línea", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
                    }
                }
            Case "FACTURA"
                config = New CrudTablaConfig With {
                    .TableName = "Factura",
                    .Pk = "factura_id",
                    .Endpoint = "Factura",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "factura_id", .Label = "ID", .Type = "hidden"},
                        New CrudCampoConfig With {
                            .Name = "orden_venta_id",
                            .Label = "Orden Venta",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Orden_Venta/Index",
                            .ValueField = "orden_venta_id",
                            .TextField = "num_orden"
                        },
                        New CrudCampoConfig With {.Name = "num_factura", .Label = "Número Factura", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "fecha_emision", .Label = "Fecha Emisión", .Type = "date", .Required = True},
                        New CrudCampoConfig With {.Name = "nit_facturacion", .Label = "NIT Facturación", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "direccion_facturacion_snapshot", .Label = "Dirección Facturación", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "total_factura_snapshot", .Label = "Total Factura", .Type = "number", .Required = True}
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