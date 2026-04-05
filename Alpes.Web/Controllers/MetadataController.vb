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
            New With {.Value = "Factura_Detalle", .Text = "Factura Detalle"},
            New With {.Value = "Devolucion", .Text = "Devolución"},
            New With {.Value = "Historial_Compra", .Text = "Historial Compra"},
            New With {.Value = "Resena_Comentario", .Text = "Reseña Comentario"},
            New With {.Value = "Regla_Envio_Gratis", .Text = "Regla Envio Gratis"},
            New With {.Value = "Tarifa_Envio", .Text = "Tarifa Envio"},
            New With {.Value = "Promocion", .Text = "Promocion"},
            New With {.Value = "Promocion_Producto", .Text = "Promocion Producto"},
            New With {.Value = "Regla_Promocion", .Text = "Regla Promocion"},
            New With {.Value = "Historial_Promocion", .Text = "Historial Promocion"},
            New With {.Value = "Expediente_Empleado", .Text = "Expediente Empleado"},
            New With {.Value = "Evaluacion", .Text = "Evaluacion"},
            New With {.Value = "Nomina", .Text = "Nomina"},
            New With {.Value = "Nomina_Detalle", .Text = "Nomina Detalle"},
            New With {.Value = "Incidente_Laboral", .Text = "Incidente Laboral"}
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

            Case "PROMOCION_PRODUCTO"
                config = New CrudTablaConfig With {
                    .TableName = "Promocion_Producto",
                    .Pk = "promocion_producto_id",
                    .Endpoint = "Promocion_Producto",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "promocion_producto_id", .Label = "ID", .Type = "hidden"},
                        New CrudCampoConfig With {
                            .Name = "promocion_id",
                            .Label = "Promocion",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Promocion/Index",
                            .ValueField = "promocion_id",
                            .TextField = "nombre"
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
                        New CrudCampoConfig With {.Name = "limite_unidades", .Label = "Limite Unidades", .Type = "number", .Required = True}
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

            Case "PROMOCION"
                config = New CrudTablaConfig With {
                    .TableName = "Promocion",
                    .Pk = "promocion_id",
                    .Endpoint = "Promocion",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "promocion_id", .Label = "ID", .Type = "hidden"},
                        New CrudCampoConfig With {
                            .Name = "tipo_promocion_id",
                            .Label = "Tipo Promocion",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Tipo_Promocion/Index",
                            .ValueField = "tipo_promocion_id",
                            .TextField = "nombre"
                        },
                        New CrudCampoConfig With {.Name = "nombre", .Label = "Nombre", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "descripcion", .Label = "Descripcion", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "vigencia_inicio", .Label = "Vigencia Inicio", .Type = "date", .Required = True},
                        New CrudCampoConfig With {.Name = "vigencia_fin", .Label = "Vigencia Fin", .Type = "date"},
                        New CrudCampoConfig With {.Name = "prioridad", .Label = "Prioridad", .Type = "number", .Required = True}
                    }
                }

            Case "REGLA_ENVIO_GRATIS"
                config = New CrudTablaConfig With {
        .TableName = "Regla_Envio_Gratis",
        .Pk = "regla_envio_gratis_id",
        .Endpoint = "Regla_Envio_Gratis",
        .Campos = New List(Of CrudCampoConfig) From {
            New CrudCampoConfig With {.Name = "regla_envio_gratis_id", .Label = "ID", .Type = "hidden"},
            New CrudCampoConfig With {
                .Name = "zona_envio_id",
                .Label = "Zona Envio",
                .Type = "select",
                .Required = True,
                .DataSource = "/Zona_Envio/Index",
                .ValueField = "zona_envio_id",
                .TextField = "nombre"
            },
            New CrudCampoConfig With {.Name = "monto_minimo", .Label = "Monto Minimo", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "peso_max_kg", .Label = "Peso Max Kg", .Type = "number", .Required = True},
            New CrudCampoConfig With {.Name = "vigencia_inicio", .Label = "Vigencia Inicio", .Type = "date", .Required = True},
            New CrudCampoConfig With {.Name = "vigencia_fin", .Label = "Vigencia Fin", .Type = "date"}
        }
    }
            Case "NOMINA_DETALLE"
                config = New CrudTablaConfig With {
                    .TableName = "Nomina_Detalle",
                    .Pk = "nomina_detalle_id",
                    .Endpoint = "Nomina_Detalle",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "nomina_detalle_id", .Label = "ID", .Type = "hidden"},
                        New CrudCampoConfig With {
                            .Name = "nomina_id",
                            .Label = "Nomina",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Nomina/Index",
                            .ValueField = "nomina_id",
                            .TextField = "nomina_id"
                        },
                        New CrudCampoConfig With {.Name = "tipo", .Label = "Tipo", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "concepto", .Label = "Concepto", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "monto", .Label = "Monto", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "FACTURA_DETALLE"
                config = New CrudTablaConfig With {
                    .TableName = "Factura_Detalle",
                    .Pk = "factura_det_id",
                    .Endpoint = "Factura_Detalle",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "factura_det_id", .Label = "ID", .Type = "hidden"},
                        New CrudCampoConfig With {
                            .Name = "factura_id",
                            .Label = "Factura",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Factura/Index",
                            .ValueField = "factura_id",
                            .TextField = "num_factura"
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
                        New CrudCampoConfig With {.Name = "total_linea", .Label = "Total Línea", .Type = "number", .Required = True}
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

            Case "HISTORIAL_PROMOCION"
                config = New CrudTablaConfig With {
                    .TableName = "Historial_Promocion",
                    .Pk = "historial_promocion_id",
                    .Endpoint = "Historial_Promocion",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "historial_promocion_id", .Label = "ID", .Type = "hidden"},
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
                            .Name = "promocion_id",
                            .Label = "Promocion",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Promocion/Index",
                            .ValueField = "promocion_id",
                            .TextField = "nombre"
                        },
                        New CrudCampoConfig With {.Name = "monto_descuento_snapshot", .Label = "Monto Descuento", .Type = "number", .Required = True}
                    }
                }
            Case "INCIDENTE_LABORAL"
                config = New CrudTablaConfig With {
                    .TableName = "Incidente_Laboral",
                    .Pk = "incidente_id",
                    .Endpoint = "Incidente_Laboral",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "incidente_id", .Label = "ID", .Type = "hidden"},
                        New CrudCampoConfig With {
                            .Name = "emp_id",
                            .Label = "Empleado",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Empleado/Index",
                            .ValueField = "emp_id",
                            .TextField = "nombres"
                        },
                        New CrudCampoConfig With {.Name = "fecha_incidente", .Label = "Fecha Incidente", .Type = "date"},
                        New CrudCampoConfig With {.Name = "descripcion", .Label = "Descripcion", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "gravedad", .Label = "Gravedad", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "acciones_tomadas", .Label = "Acciones", .Type = "text"},
                        New CrudCampoConfig With {.Name = "estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "RESENA_COMENTARIO"
                config = New CrudTablaConfig With {
                    .TableName = "Resena_Comentario",
                    .Pk = "resena_id",
                    .Endpoint = "Resena_Comentario",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "resena_id", .Label = "ID", .Type = "hidden"},
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
                            .Name = "producto_id",
                            .Label = "Producto",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Producto/Index",
                            .ValueField = "producto_id",
                            .TextField = "nombre"
                        },
                        New CrudCampoConfig With {.Name = "calificacion", .Label = "Calificacion", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "comentario", .Label = "Comentario", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "resena_at", .Label = "Resena", .Type = "datetime-local", .Required = True}
                    }
                }

            Case "REGLA_PROMOCION"
                config = New CrudTablaConfig With {
                    .TableName = "Regla_Promocion",
                    .Pk = "regla_promocion_id",
                    .Endpoint = "Regla_Promocion",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "regla_promocion_id", .Label = "ID", .Type = "hidden"},
                        New CrudCampoConfig With {
                            .Name = "promocion_id",
                            .Label = "Promocion",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Promocion/Index",
                            .ValueField = "promocion_id",
                            .TextField = "nombre"
                        },
                        New CrudCampoConfig With {.Name = "min_compra", .Label = "Min Compra", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "min_items", .Label = "Min Items", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "aplica_tipo_producto", .Label = "Aplica Tipo Producto", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "tope_descuento", .Label = "Tope Descuento", .Type = "number", .Required = True}
                    }
                }

            Case "DEVOLUCION"
                config = New CrudTablaConfig With {
                    .TableName = "Devolucion",
                    .Pk = "devolucion_id",
                    .Endpoint = "Devolucion",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "devolucion_id", .Label = "ID", .Type = "hidden"},
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
                            .Name = "cli_id",
                            .Label = "Cliente",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Cliente/Index",
                            .ValueField = "cli_id",
                            .TextField = "nombres"
                        },
                        New CrudCampoConfig With {.Name = "motivo", .Label = "Motivo", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "estado_devolucion", .Label = "Estado Devolución", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "solicitud_at", .Label = "Solicitud", .Type = "datetime-local", .Required = True},
                        New CrudCampoConfig With {.Name = "resolucion_at", .Label = "Resolución", .Type = "datetime-local", .Nullable = True}
                    }
                }

            Case "TARIFA_ENVIO"
                config = New CrudTablaConfig With {
                    .TableName = "Tarifa_Envio",
                    .Pk = "tarifa_envio_id",
                    .Endpoint = "Tarifa_Envio",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "tarifa_envio_id", .Label = "ID", .Type = "hidden"},
                        New CrudCampoConfig With {
                            .Name = "zona_envio_id",
                            .Label = "Zona Envio",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Zona_Envio/Index",
                            .ValueField = "zona_envio_id",
                            .TextField = "nombre"
                        },
                        New CrudCampoConfig With {
                            .Name = "tipo_entrega_id",
                            .Label = "Tipo Entrega",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Tipo_Entrega/Index",
                            .ValueField = "tipo_entrega_id",
                            .TextField = "nombre"
                        },
                        New CrudCampoConfig With {.Name = "peso_desde_kg", .Label = "Peso Desde Kg", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "peso_hasta_kg", .Label = "Peso Hasta Kg", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "costo", .Label = "Costo", .Type = "number", .Required = True}
                    }
                }

            Case "HISTORIAL_COMPRA"
                config = New CrudTablaConfig With {
                    .TableName = "Historial_Compra",
                    .Pk = "hist_compra_id",
                    .Endpoint = "Historial_Compra",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "hist_compra_id", .Label = "ID", .Type = "hidden"},
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
                            .Name = "orden_venta_id",
                            .Label = "Orden Venta",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Orden_Venta/Index",
                            .ValueField = "orden_venta_id",
                            .TextField = "num_orden"
                        },
                        New CrudCampoConfig With {.Name = "compra_at", .Label = "Compra", .Type = "datetime-local", .Required = True},
                        New CrudCampoConfig With {.Name = "monto_total_snapshot", .Label = "Monto Total", .Type = "number", .Required = True}
                    }
                }

            Case "EXPEDIENTE_EMPLEADO"
                config = New CrudTablaConfig With {
                    .TableName = "Expediente_Empleado",
                    .Pk = "expediente_empleado_id",
                    .Endpoint = "Expediente_Empleado",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "expediente_empleado_id", .Label = "ID", .Type = "hidden"},
                        New CrudCampoConfig With {
                            .Name = "emp_id",
                            .Label = "Empleado",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Empleado/Index",
                            .ValueField = "emp_id",
                            .TextField = "nombres"
                        },
                        New CrudCampoConfig With {.Name = "tipo_documento", .Label = "Tipo Documento", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "url_documento", .Label = "URL Documento", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "fecha_documento", .Label = "Fecha Documento", .Type = "date"},
                        New CrudCampoConfig With {.Name = "estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "EVALUACION"
                config = New CrudTablaConfig With {
                    .TableName = "Evaluacion",
                    .Pk = "evaluacion_id",
                    .Endpoint = "Evaluacion",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "evaluacion_id", .Label = "ID", .Type = "hidden"},
                        New CrudCampoConfig With {
                            .Name = "emp_id",
                            .Label = "Empleado",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Empleado/Index",
                            .ValueField = "emp_id",
                            .TextField = "nombres"
                        },
                        New CrudCampoConfig With {
                            .Name = "evaluador_emp_id",
                            .Label = "Evaluador",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Empleado/Index",
                            .ValueField = "emp_id",
                            .TextField = "nombres"
                        },
                        New CrudCampoConfig With {.Name = "fecha_eval", .Label = "Fecha Evaluacion", .Type = "date"},
                        New CrudCampoConfig With {.Name = "puntuacion", .Label = "Puntuacion", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "comentarios", .Label = "Comentarios", .Type = "text"},
                        New CrudCampoConfig With {.Name = "estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "NOMINA"
                config = New CrudTablaConfig With {
                    .TableName = "Nomina",
                    .Pk = "nomina_id",
                    .Endpoint = "Nomina",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "nomina_id", .Label = "ID", .Type = "hidden"},
                        New CrudCampoConfig With {
                            .Name = "emp_id",
                            .Label = "Empleado",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Empleado/Index",
                            .ValueField = "emp_id",
                            .TextField = "nombres"
                        },
                        New CrudCampoConfig With {.Name = "periodo_inicio", .Label = "Periodo Inicio", .Type = "date"},
                        New CrudCampoConfig With {.Name = "periodo_fin", .Label = "Periodo Fin", .Type = "date"},
                        New CrudCampoConfig With {.Name = "monto_bruto", .Label = "Monto Bruto", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "monto_neto", .Label = "Monto Neto", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "fecha_pago", .Label = "Fecha Pago", .Type = "date"},
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