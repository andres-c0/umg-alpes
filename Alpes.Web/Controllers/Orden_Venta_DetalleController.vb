Option Strict On
Option Explicit On

Imports System.Web.Mvc

Public Class Orden_Venta_DetalleController
    Inherits Controller

    Function ListarTablas() As ActionResult
        Dim tablas = New List(Of Object) From {
            New With {.Value = "Usuario", .Text = "Usuario"},
            New With {.Value = "Rol", .Text = "Rol"},
            New With {.Value = "Orden_Venta", .Text = "Orden Venta"},
            New With {.Value = "Orden_Venta_Detalle", .Text = "Orden Venta Detalle"}
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
                        New CrudCampoConfig With {.Name = "Email", .Label = "Email", .Type = "text"},
                        New CrudCampoConfig With {.Name = "Telefono", .Label = "Telefono", .Type = "text"},
                        New CrudCampoConfig With {
                            .Name = "RolId",
                            .Label = "Rol",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Rol/Index",
                            .ValueField = "RolId",
                            .TextField = "RolNombre"
                        },
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .DefaultValue = "ACTIVO"}
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
                        New CrudCampoConfig With {.Name = "Descripcion", .Label = "Descripcion", .Type = "text"},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .DefaultValue = "ACTIVO"}
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
                        New CrudCampoConfig With {.Name = "descuento", .Label = "Descuento", .Type = "number"},
                        New CrudCampoConfig With {.Name = "impuesto", .Label = "Impuesto", .Type = "number"},
                        New CrudCampoConfig With {.Name = "total", .Label = "Total", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "moneda", .Label = "Moneda", .Type = "text"},
                        New CrudCampoConfig With {.Name = "direccion_envio_snapshot", .Label = "Dirección", .Type = "text"},
                        New CrudCampoConfig With {.Name = "observaciones", .Label = "Observaciones", .Type = "text"},
                        New CrudCampoConfig With {.Name = "estado", .Label = "Estado", .Type = "text", .DefaultValue = "ACTIVO"}
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
                        New CrudCampoConfig With {.Name = "estado", .Label = "Estado", .Type = "text", .DefaultValue = "ACTIVO"}
                    }
                }

        End Select

        If config Is Nothing Then
            Return Json(New With {
                .success = False,
                .message = "No existe metadata para la tabla solicitada."
            }, JsonRequestBehavior.AllowGet)
        End If

        Return Json(config, JsonRequestBehavior.AllowGet)
    End Function

End Class