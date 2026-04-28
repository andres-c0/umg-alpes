Option Strict On
Option Explicit On

Imports System.Web.Mvc

Public Class MetadataController
    Inherits Controller

    Function ListarTablas() As ActionResult
        Dim tablas = New List(Of Object) From {
            New With {.Value = "Usuario", .Text = "Usuario"},
            New With {.Value = "Rol", .Text = "Rol"},
            New With {.Value = "Permiso", .Text = "Permiso"},
            New With {.Value = "Rol_Permiso", .Text = "Rol_Permiso"},
            New With {.Value = "Departamento", .Text = "Departamento"},
            New With {.Value = "Cargo", .Text = "Cargo"},
            New With {.Value = "Rol_Empleado", .Text = "Rol_Empleado"},
            New With {.Value = "Cliente", .Text = "Cliente"},
            New With {.Value = "Unidad_Medida", .Text = "Unidad_Medida"},
            New With {.Value = "Categoria", .Text = "Categoria"},
            New With {.Value = "Estado_Produccion", .Text = "Estado_Produccion"},
            New With {.Value = "Estado_Orden_Compra", .Text = "Estado_Orden_Compra"},
            New With {.Value = "Estado_Orden", .Text = "Estado_Orden"},
            New With {.Value = "Metodo_Pago", .Text = "Metodo_Pago"},
            New With {.Value = "Estado_Envio", .Text = "Estado_Envio"},
            New With {.Value = "Tipo_Entrega", .Text = "Tipo_Entrega"},
            New With {.Value = "Zona_Envio", .Text = "Zona_Envio"},
            New With {.Value = "Politica_Envio", .Text = "Politica_Envio"},
            New With {.Value = "Regla_Envio_Gratis", .Text = "Regla_Envio_Gratis"},
            New With {.Value = "Tarifa_Envio", .Text = "Tarifa_Envio"},
            New With {.Value = "Tipo_Promocion", .Text = "Tipo_Promocion"},
            New With {.Value = "Promocion", .Text = "Promocion"},
            New With {.Value = "PromocionProducto", .Text = "PromocionProducto"},
            New With {.Value = "ReglaPromocion", .Text = "ReglaPromocion"},
            New With {.Value = "Campana_Marketing", .Text = "Campana_Marketing"},
            New With {.Value = "Cupon", .Text = "Cupon"},
            New With {.Value = "HistorialPromocion", .Text = "HistorialPromocion"},
            New With {.Value = "Orden_Venta", .Text = "Orden Venta"},
            New With {.Value = "Orden_Venta_Detalle", .Text = "Orden Venta Detalle"},
            New With {.Value = "Pago", .Text = "Pago"},
            New With {.Value = "Cuotas_Pago", .Text = "Cuotas Pago"},
            New With {.Value = "Factura", .Text = "Factura"},
            New With {.Value = "Factura_Detalle", .Text = "Factura_Detalle"},
            New With {.Value = "Devolucion", .Text = "Devolucion"},
            New With {.Value = "Historial_Compra", .Text = "Historial_Compra"},
            New With {.Value = "Resena_Comentario", .Text = "Resena_Comentario"},
            New With {.Value = "FAQ", .Text = "FAQ"},
            New With {.Value = "ExpedienteEmpleado", .Text = "ExpedienteEmpleado"},
            New With {.Value = "Evaluacion", .Text = "Evaluacion"},
            New With {.Value = "Nomina", .Text = "Nomina"},
            New With {.Value = "NominaDetalle", .Text = "NominaDetalle"},
            New With {.Value = "IncidenteLaboral", .Text = "IncidenteLaboral"},
            New With {.Value = "HistorialLaboral", .Text = "HistorialLaboral"},
            New With {.Value = "SeguimientoEnvio", .Text = "SeguimientoEnvio"},
            New With {.Value = "PlanProduccion", .Text = "Plan_Produccion"},
            New With {.Value = "OrdenProduccion", .Text = "Orden_Produccion"},
            New With {.Value = "ListaMateriales", .Text = "Lista_Materiales"},
            New With {.Value = "ListaMaterialesDetalle", .Text = "Lista_Materiales_Detalle"},
            New With {.Value = "Herramienta", .Text = "Herramienta"},
            New With {.Value = "MantenimientoHerramienta", .Text = "Mantenimiento_Herramienta"},
            New With {.Value = "OrdenProduccionTarea", .Text = "Orden_Produccion_Tarea"},
            New With {.Value = "ConsumoMateriaPrima", .Text = "Consumo_Materia_Prima"},
            New With {.Value = "ProduccionResultados", .Text = "Produccion_Resultados"},
            New With {.Value = "CondicionPago", .Text = "Condicion_Pago"},
            New With {.Value = "Abastecimiento", .Text = "Abastecimiento"},
            New With {.Value = "CuentaPagarProveedor", .Text = "Cuenta_Pagar_Proveedor"},
            New With {.Value = "PagoProveedor", .Text = "Pago_Proveedor"},
            New With {.Value = "ContratoProveedor", .Text = "Contrato_Proveedor"},
            New With {.Value = "ExpedienteProveedor", .Text = "Expediente_Proveedor"},
            New With {.Value = "HistoricoPrecioMateriaPrimaProveedor", .Text = "Historico_Precio_Materia_Prima_Proveedor"},
            New With {.Value = "PreferenciaCliente", .Text = "Preferencia_Cliente"},
            New With {.Value = "Carrito", .Text = "Carrito"},
            New With {.Value = "CarritoDetalle", .Text = "Carrito_Detalle"},
            New With {.Value = "ListaDeseos", .Text = "Lista_Deseos"},
            New With {.Value = "Empleado", .Text = "Empleado"},
            New With {.Value = "Producto", .Text = "Producto"},
            New With {.Value = "Inventario_Producto", .Text = "Inventario_Producto"},
            New With {.Value = "Movimiento_Inventario", .Text = "Movimiento_Inventario"},
            New With {.Value = "Precio_Historico", .Text = "Precio_Historico"},
            New With {.Value = "Materia_Prima", .Text = "Materia_Prima"},
            New With {.Value = "Inventario_Materia_Prima", .Text = "Inventario_Materia_Prima"},
            New With {.Value = "Movimiento_Materia_Prima", .Text = "Movimiento_Materia_Prima"},
            New With {.Value = "Proveedor", .Text = "Proveedor"},
            New With {.Value = "Orden_Compra", .Text = "Orden_Compra"},
            New With {.Value = "Orden_Compra_Detalle", .Text = "Orden_Compra_Detalle"},
            New With {.Value = "Recepcion_Material", .Text = "Recepcion_Material"},
            New With {.Value = "Control_Calidad", .Text = "Control_Calidad"},
            New With {.Value = "Sesion", .Text = "Sesion"},
            New With {.Value = "Historial_Cambio_Contrasena", .Text = "Historial_Cambio_Contrasena"},
            New With {.Value = "Vehiculo", .Text = "Vehiculo"},
            New With {.Value = "Ruta_Entrega", .Text = "Ruta_Entrega"}
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

            Case "PERMISO"
                config = New CrudTablaConfig With {
                    .TableName = "Permiso",
                    .Pk = "PermisoId",
                    .Endpoint = "Permiso",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "PermisoId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "Codigo", .Label = "Codigo", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Descripcion", .Label = "Descripcion", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Modulo", .Label = "Modulo", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "ROL_PERMISO"
                config = New CrudTablaConfig With {
                    .TableName = "Rol_Permiso",
                    .Pk = "RolPermisoId",
                    .Endpoint = "Rol_Permiso",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "RolPermisoId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {
                            .Name = "RolId",
                            .Label = "Rol",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Rol/Index",
                            .ValueField = "RolId",
                            .TextField = "RolNombre"
                        },
                        New CrudCampoConfig With {
                            .Name = "PermisoId",
                            .Label = "Permiso",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Permiso/Index",
                            .ValueField = "PermisoId",
                            .TextField = "Codigo"
                        },
                        New CrudCampoConfig With {.Name = "EsPermitido", .Label = "Es Permitido", .Type = "number", .Required = True, .DefaultValue = "1"},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "DEPARTAMENTO"
                config = New CrudTablaConfig With {
                    .TableName = "Departamento",
                    .Pk = "DeptoId",
                    .Endpoint = "Departamento",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "DeptoId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "Nombre", .Label = "Nombre", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Descripcion", .Label = "Descripcion", .Type = "text", .Nullable = True}
                    }
                }

            Case "CARGO"
                config = New CrudTablaConfig With {
                    .TableName = "Cargo",
                    .Pk = "CargoId",
                    .Endpoint = "Cargo",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "CargoId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "Nombre", .Label = "Nombre", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Descripcion", .Label = "Descripcion", .Type = "text", .Nullable = True}
                    }
                }

            Case "ROL_EMPLEADO"
                config = New CrudTablaConfig With {
                    .TableName = "Rol_Empleado",
                    .Pk = "RolEmpleadoId",
                    .Endpoint = "Rol_Empleado",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "RolEmpleadoId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "Nombre", .Label = "Nombre", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Descripcion", .Label = "Descripcion", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "CLIENTE"
                config = New CrudTablaConfig With {
                    .TableName = "Cliente",
                    .Pk = "CliId",
                    .Endpoint = "Cliente",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "CliId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "TipoDocumento", .Label = "Tipo Documento", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "NumDocumento", .Label = "Numero Documento", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Nit", .Label = "NIT", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Nombres", .Label = "Nombres", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Apellidos", .Label = "Apellidos", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Email", .Label = "Email", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "TelResidencia", .Label = "Telefono Residencia", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "TelCelular", .Label = "Telefono Celular", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Direccion", .Label = "Direccion", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Ciudad", .Label = "Ciudad", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Departamento", .Label = "Departamento", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Pais", .Label = "Pais", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Profesion", .Label = "Profesion", .Type = "text", .Nullable = True}
                    }
                }

            Case "UNIDAD_MEDIDA"
                config = New CrudTablaConfig With {
                    .TableName = "Unidad_Medida",
                    .Pk = "UnidadMedidaId",
                    .Endpoint = "Unidad_Medida",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "UnidadMedidaId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "Codigo", .Label = "Codigo", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Nombre", .Label = "Nombre", .Type = "text", .Required = True}
                    }
                }

            Case "CATEGORIA"
                config = New CrudTablaConfig With {
                    .TableName = "Categoria",
                    .Pk = "CategoriaId",
                    .Endpoint = "Categoria",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "CategoriaId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "Nombre", .Label = "Nombre", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Descripcion", .Label = "Descripcion", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {
                            .Name = "CategoriaPadreId",
                            .Label = "Categoria Padre",
                            .Type = "select",
                            .Nullable = True,
                            .DataSource = "/Categoria/Index",
                            .ValueField = "CategoriaId",
                            .TextField = "Nombre"
                        }
                    }
                }

            Case "ESTADO_PRODUCCION"
                config = New CrudTablaConfig With {
                    .TableName = "Estado_Produccion",
                    .Pk = "EstadoProduccionId",
                    .Endpoint = "Estado_Produccion",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "EstadoProduccionId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "Codigo", .Label = "Codigo", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Descripcion", .Label = "Descripcion", .Type = "text", .Nullable = True}
                    }
                }

            Case "ESTADO_ORDEN_COMPRA"
                config = New CrudTablaConfig With {
                    .TableName = "Estado_Orden_Compra",
                    .Pk = "EstadoOcId",
                    .Endpoint = "Estado_Orden_Compra",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "EstadoOcId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "Codigo", .Label = "Codigo", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Descripcion", .Label = "Descripcion", .Type = "text", .Nullable = True}
                    }
                }

            Case "ESTADO_ORDEN"
                config = New CrudTablaConfig With {
                    .TableName = "Estado_Orden",
                    .Pk = "EstadoOrdenId",
                    .Endpoint = "Estado_Orden",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "EstadoOrdenId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "Codigo", .Label = "Codigo", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Descripcion", .Label = "Descripcion", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "METODO_PAGO"
                config = New CrudTablaConfig With {
                    .TableName = "Metodo_Pago",
                    .Pk = "MetodoPagoId",
                    .Endpoint = "Metodo_Pago",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "MetodoPagoId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "Nombre", .Label = "Nombre", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "RequiereDatosExtra", .Label = "Requiere Datos Extra", .Type = "number", .Required = True, .DefaultValue = "0"},
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

            Case "ESTADO_ENVIO"
                config = New CrudTablaConfig With {
                    .TableName = "Estado_Envio",
                    .Pk = "EstadoEnvioId",
                    .Endpoint = "Estado_Envio",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "EstadoEnvioId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "Codigo", .Label = "Codigo", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Descripcion", .Label = "Descripcion", .Type = "text", .Nullable = True}
                    }
                }

            Case "TIPO_ENTREGA"
                config = New CrudTablaConfig With {
                    .TableName = "Tipo_Entrega",
                    .Pk = "TipoEntregaId",
                    .Endpoint = "Tipo_Entrega",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "TipoEntregaId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "Nombre", .Label = "Nombre", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Descripcion", .Label = "Descripcion", .Type = "text", .Nullable = True}
                    }
                }

            Case "ZONA_ENVIO"
                config = New CrudTablaConfig With {
                    .TableName = "Zona_Envio",
                    .Pk = "ZonaEnvioId",
                    .Endpoint = "Zona_Envio",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "ZonaEnvioId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "Nombre", .Label = "Nombre", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Pais", .Label = "Pais", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Departamento", .Label = "Departamento", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Ciudad", .Label = "Ciudad", .Type = "text", .Nullable = True}
                    }
                }

            Case "POLITICA_ENVIO"
                config = New CrudTablaConfig With {
                    .TableName = "Politica_Envio",
                    .Pk = "PoliticaEnvioId",
                    .Endpoint = "Politica_Envio",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "PoliticaEnvioId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "Titulo", .Label = "Titulo", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Descripcion", .Label = "Descripcion", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "VigenciaInicio", .Label = "Vigencia Inicio", .Type = "date", .Nullable = True},
                        New CrudCampoConfig With {.Name = "VigenciaFin", .Label = "Vigencia Fin", .Type = "date", .Nullable = True}
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

            Case "TIPO_PROMOCION"
                config = New CrudTablaConfig With {
                    .TableName = "Tipo_Promocion",
                    .Pk = "TipoPromocionId",
                    .Endpoint = "Tipo_Promocion",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "TipoPromocionId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "Nombre", .Label = "Nombre", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Descripcion", .Label = "Descripcion", .Type = "text", .Nullable = True}
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

            Case "CAMPANA_MARKETING"
                config = New CrudTablaConfig With {
                    .TableName = "Campana_Marketing",
                    .Pk = "CampanaMarketingId",
                    .Endpoint = "Campana_Marketing",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "CampanaMarketingId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "Nombre", .Label = "Nombre", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Canal", .Label = "Canal", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Presupuesto", .Label = "Presupuesto", .Type = "number", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Inicio", .Label = "Inicio", .Type = "date", .Required = True},
                        New CrudCampoConfig With {.Name = "Fin", .Label = "Fin", .Type = "date", .Required = True}
                    }
                }

            Case "CUPON"
                config = New CrudTablaConfig With {
                    .TableName = "Cupon",
                    .Pk = "CuponId",
                    .Endpoint = "Cupon",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "CuponId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "Codigo", .Label = "Codigo", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Descripcion", .Label = "Descripcion", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "VigenciaInicio", .Label = "Vigencia Inicio", .Type = "date", .Required = True},
                        New CrudCampoConfig With {.Name = "VigenciaFin", .Label = "Vigencia Fin", .Type = "date", .Required = True},
                        New CrudCampoConfig With {.Name = "LimiteUsoTotal", .Label = "Limite Uso Total", .Type = "number", .Nullable = True},
                        New CrudCampoConfig With {.Name = "LimiteUsoPorCliente", .Label = "Limite Uso Por Cliente", .Type = "number", .Nullable = True},
                        New CrudCampoConfig With {.Name = "UsosActuales", .Label = "Usos Actuales", .Type = "number", .Required = True, .DefaultValue = "0"}
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

            Case "FAQ"
                config = New CrudTablaConfig With {
                    .TableName = "FAQ",
                    .Pk = "FaqId",
                    .Endpoint = "FAQ",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "FaqId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "Pregunta", .Label = "Pregunta", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Respuesta", .Label = "Respuesta", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Orden", .Label = "Orden", .Type = "number", .Nullable = True, .DefaultValue = "0"}
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
            Case "HISTORIALLABORAL"
                config = New CrudTablaConfig With {
                    .TableName = "Historial_Laboral",
                    .Pk = "HistorialLaboralId",
                    .Endpoint = "HistorialLaboral",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "HistorialLaboralId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "EmpId", .Label = "Empleado Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "FechaCambio", .Label = "Fecha Cambio", .Type = "date", .Required = True},
                        New CrudCampoConfig With {.Name = "TipoCambio", .Label = "Tipo Cambio", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Detalle", .Label = "Detalle", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
                    }
                }
            Case "SEGUIMIENTOENVIO"
                config = New CrudTablaConfig With {
                    .TableName = "Seguimiento_Envio",
                    .Pk = "SegEnvioId",
                    .Endpoint = "SeguimientoEnvio",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "SegEnvioId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "EnvioId", .Label = "Envío Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "EstadoEnvioId", .Label = "Estado Envío Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "EventoAt", .Label = "Fecha Evento", .Type = "datetime-local", .Required = True},
                        New CrudCampoConfig With {.Name = "UbicacionTexto", .Label = "Ubicación", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Observacion", .Label = "Observación", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Required = False, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "PLANPRODUCCION", "PLAN_PRODUCCION"
                config = New CrudTablaConfig With {
                    .TableName = "PlanProduccion",
                    .Pk = "PlanProduccionId",
                    .Endpoint = "PlanProduccion",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "PlanProduccionId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {
                            .Name = "ProductoId",
                            .Label = "Producto",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Producto/Index",
                            .ValueField = "ProductoId",
                            .TextField = "Nombre"
                        },
                        New CrudCampoConfig With {.Name = "Cantidad", .Label = "Cantidad", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "PeriodoInicio", .Label = "Periodo Inicio", .Type = "date", .Required = True},
                        New CrudCampoConfig With {.Name = "PeriodoFin", .Label = "Periodo Fin", .Type = "date", .Required = True},
                        New CrudCampoConfig With {.Name = "TiempoEstimadoHoras", .Label = "Tiempo Estimado Horas", .Type = "number", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Nullable = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "ORDENPRODUCCION"
                config = New CrudTablaConfig With {
                    .TableName = "OrdenProduccion",
                    .Pk = "OrdenProduccionId",
                    .Endpoint = "OrdenProduccion",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "OrdenProduccionId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "NumOp", .Label = "Numero OP", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "ProductoId", .Label = "Producto Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "CantidadPlanificada", .Label = "Cantidad Planificada", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "EstadoProduccionId", .Label = "Estado Produccion Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "InicioEstimado", .Label = "Inicio Estimado", .Type = "date", .Nullable = True},
                        New CrudCampoConfig With {.Name = "FinEstimado", .Label = "Fin Estimado", .Type = "date", .Nullable = True},
                        New CrudCampoConfig With {.Name = "InicioReal", .Label = "Inicio Real", .Type = "date", .Nullable = True},
                        New CrudCampoConfig With {.Name = "FinReal", .Label = "Fin Real", .Type = "date", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Nullable = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "LISTAMATERIALES"
                config = New CrudTablaConfig With {
                    .TableName = "ListaMateriales",
                    .Pk = "ListaMaterialesId",
                    .Endpoint = "ListaMateriales",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "ListaMaterialesId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "ProductoId", .Label = "Producto Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Version", .Label = "Versión", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "VigenciaInicio", .Label = "Vigencia Inicio", .Type = "date", .Nullable = True},
                        New CrudCampoConfig With {.Name = "VigenciaFin", .Label = "Vigencia Fin", .Type = "date", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Nullable = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "LISTAMATERIALESDETALLE"
                config = New CrudTablaConfig With {
                    .TableName = "ListaMaterialesDetalle",
                    .Pk = "ListaMaterialesDetId",
                    .Endpoint = "ListaMaterialesDetalle",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "ListaMaterialesDetId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "ListaMaterialesId", .Label = "Lista Materiales Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "MpId", .Label = "Materia Prima Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "CantidadRequerida", .Label = "Cantidad Requerida", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "MermaPct", .Label = "Merma %", .Type = "number", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Nullable = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "HERRAMIENTA"
                config = New CrudTablaConfig With {
                    .TableName = "Herramienta",
                    .Pk = "HerramientaId",
                    .Endpoint = "Herramienta",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "HerramientaId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "Codigo", .Label = "Codigo", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Nombre", .Label = "Nombre", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Descripcion", .Label = "Descripcion", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "FechaCompra", .Label = "Fecha Compra", .Type = "date", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Nullable = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "MANTENIMIENTOHERRAMIENTA"
                config = New CrudTablaConfig With {
                    .TableName = "MantenimientoHerramienta",
                    .Pk = "MantenimientoId",
                    .Endpoint = "MantenimientoHerramienta",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "MantenimientoId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {
                            .Name = "HerramientaId",
                            .Label = "Herramienta",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Herramienta/Index",
                            .ValueField = "HerramientaId",
                            .TextField = "Nombre"
                        },
                        New CrudCampoConfig With {.Name = "FechaMantenimiento", .Label = "Fecha Mantenimiento", .Type = "date", .Required = True},
                        New CrudCampoConfig With {.Name = "Tipo", .Label = "Tipo", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Costo", .Label = "Costo", .Type = "number", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Observacion", .Label = "Observacion", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Nullable = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "ORDENPRODUCCIONTAREA"
                config = New CrudTablaConfig With {
                    .TableName = "OrdenProduccionTarea",
                    .Pk = "OpTareaId",
                    .Endpoint = "OrdenProduccionTarea",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "OpTareaId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "OrdenProduccionId", .Label = "Orden Produccion Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "NombreTarea", .Label = "Nombre Tarea", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Orden", .Label = "Orden", .Type = "number", .Nullable = True},
                        New CrudCampoConfig With {.Name = "InicioAt", .Label = "Inicio", .Type = "datetime-local", .Nullable = True},
                        New CrudCampoConfig With {.Name = "FinAt", .Label = "Fin", .Type = "datetime-local", .Nullable = True},
                        New CrudCampoConfig With {.Name = "EmpIdResponsable", .Label = "Empleado Responsable Id", .Type = "number", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Nullable = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "CONSUMOMATERIAPRIMA"
                config = New CrudTablaConfig With {
                    .TableName = "ConsumoMateriaPrima",
                    .Pk = "ConsumoId",
                    .Endpoint = "ConsumoMateriaPrima",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "ConsumoId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "OrdenProduccionId", .Label = "Orden Produccion Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "MpId", .Label = "Materia Prima Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Cantidad", .Label = "Cantidad", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "ConsumoAt", .Label = "Fecha Consumo", .Type = "datetime-local", .Required = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Nullable = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "PRODUCCIONRESULTADOS"
                config = New CrudTablaConfig With {
                    .TableName = "ProduccionResultados",
                    .Pk = "ResultadoId",
                    .Endpoint = "ProduccionResultados",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "ResultadoId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "OrdenProduccionId", .Label = "Orden Produccion Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "UnidadesBuenas", .Label = "Unidades Buenas", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "UnidadesMerma", .Label = "Unidades Merma", .Type = "number", .Required = True, .DefaultValue = "0"},
                        New CrudCampoConfig With {.Name = "RegistroAt", .Label = "Fecha Registro", .Type = "datetime-local", .Required = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Nullable = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "CONDICIONPAGO"
                config = New CrudTablaConfig With {
                    .TableName = "CondicionPago",
                    .Pk = "CondicionPagoId",
                    .Endpoint = "CondicionPago",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "CondicionPagoId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "Nombre", .Label = "Nombre", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "DiasCredito", .Label = "Dias Credito", .Type = "number", .Required = True, .DefaultValue = "0"},
                        New CrudCampoConfig With {.Name = "Descripcion", .Label = "Descripcion", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Nullable = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "ABASTECIMIENTO"
                config = New CrudTablaConfig With {
                    .TableName = "Abastecimiento",
                    .Pk = "AbastecimientoId",
                    .Endpoint = "Abastecimiento",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "AbastecimientoId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "MpId", .Label = "Materia Prima Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "CantidadSugerida", .Label = "Cantidad Sugerida", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Motivo", .Label = "Motivo", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Nullable = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "CUENTAPAGARPROVEEDOR"
                config = New CrudTablaConfig With {
                    .TableName = "CuentaPagarProveedor",
                    .Pk = "CuentaPagarId",
                    .Endpoint = "CuentaPagarProveedor",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "CuentaPagarId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "ProvId", .Label = "Proveedor Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "OrdenCompraId", .Label = "Orden Compra Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Saldo", .Label = "Saldo", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "FechaVencimiento", .Label = "Fecha Vencimiento", .Type = "date", .Nullable = True},
                        New CrudCampoConfig With {.Name = "EstadoCp", .Label = "Estado Cuenta Pagar", .Type = "text", .Required = True, .DefaultValue = "ABIERTA"},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Nullable = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "PAGOPROVEEDOR"
                config = New CrudTablaConfig With {
                    .TableName = "PagoProveedor",
                    .Pk = "PagoProveedorId",
                    .Endpoint = "PagoProveedor",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "PagoProveedorId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "CuentaPagarId", .Label = "Cuenta Pagar Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Monto", .Label = "Monto", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "FechaPago", .Label = "Fecha Pago", .Type = "date", .Required = True},
                        New CrudCampoConfig With {.Name = "Referencia", .Label = "Referencia", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Nullable = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "CONTRATOPROVEEDOR"
                config = New CrudTablaConfig With {
                    .TableName = "ContratoProveedor",
                    .Pk = "ContratoProveedorId",
                    .Endpoint = "ContratoProveedor",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "ContratoProveedorId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "ProvId", .Label = "Proveedor Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Titulo", .Label = "Titulo", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "VigenciaInicio", .Label = "Vigencia Inicio", .Type = "date", .Nullable = True},
                        New CrudCampoConfig With {.Name = "VigenciaFin", .Label = "Vigencia Fin", .Type = "date", .Nullable = True},
                        New CrudCampoConfig With {.Name = "UrlDocumento", .Label = "URL Documento", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Nullable = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "EXPEDIENTEPROVEEDOR"
                config = New CrudTablaConfig With {
                    .TableName = "ExpedienteProveedor",
                    .Pk = "ExpedienteProveedorId",
                    .Endpoint = "ExpedienteProveedor",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "ExpedienteProveedorId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "ProvId", .Label = "Proveedor Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "TipoDocumento", .Label = "Tipo Documento", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "UrlDocumento", .Label = "URL Documento", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "FechaDocumento", .Label = "Fecha Documento", .Type = "date", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Nullable = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "HISTORICOPRECIOMATERIAPRIMAPROVEEDOR"
                config = New CrudTablaConfig With {
                    .TableName = "HistoricoPrecioMateriaPrimaProveedor",
                    .Pk = "HistMpProvId",
                    .Endpoint = "HistoricoPrecioMateriaPrimaProveedor",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "HistMpProvId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "ProvId", .Label = "Proveedor Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "MpId", .Label = "Materia Prima Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Precio", .Label = "Precio", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "VigenciaInicio", .Label = "Vigencia Inicio", .Type = "date", .Required = True},
                        New CrudCampoConfig With {.Name = "VigenciaFin", .Label = "Vigencia Fin", .Type = "date", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Nullable = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "PREFERENCIACLIENTE"
                config = New CrudTablaConfig With {
                    .TableName = "PreferenciaCliente",
                    .Pk = "PreferenciaId",
                    .Endpoint = "PreferenciaCliente",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "PreferenciaId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "CliId", .Label = "Cliente Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "CategoriaId", .Label = "Categoria Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Estilo", .Label = "Estilo", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "MaterialPreferido", .Label = "Material Preferido", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Nullable = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "CARRITO"
                config = New CrudTablaConfig With {
                    .TableName = "Carrito",
                    .Pk = "CarritoId",
                    .Endpoint = "Carrito",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "CarritoId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "CliId", .Label = "Cliente Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "EstadoCarrito", .Label = "Estado Carrito", .Type = "text", .Nullable = True, .DefaultValue = "ACTIVO"},
                        New CrudCampoConfig With {.Name = "UltimoCalculoAt", .Label = "Ultimo Calculo", .Type = "datetime-local", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Nullable = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "CARRITODETALLE"
                config = New CrudTablaConfig With {
                    .TableName = "CarritoDetalle",
                    .Pk = "CarritoDetId",
                    .Endpoint = "CarritoDetalle",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "CarritoDetId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "CarritoId", .Label = "Carrito Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "ProductoId", .Label = "Producto Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Cantidad", .Label = "Cantidad", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "PrecioUnitarioSnapshot", .Label = "Precio Unitario Snapshot", .Type = "number", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Nullable = True, .DefaultValue = "ACTIVO"}
                    }
                }


            Case "LISTADESEOS"
                config = New CrudTablaConfig With {
                    .TableName = "ListaDeseos",
                    .Pk = "ListaDeseosId",
                    .Endpoint = "ListaDeseos",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "ListaDeseosId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "CliId", .Label = "Cliente Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "ProductoId", .Label = "Producto Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Nota", .Label = "Nota", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Nullable = True, .DefaultValue = "ACTIVO"}
                    }
                }



            Case "EMPLEADO"
                config = New CrudTablaConfig With {
                    .TableName = "Empleado",
                    .Pk = "EmpId",
                    .Endpoint = "Empleado",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "EmpId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "DeptoId", .Label = "Departamento", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "CargoId", .Label = "Cargo", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "RolEmpleadoId", .Label = "Rol Empleado", .Type = "number", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Nombres", .Label = "Nombres", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Apellidos", .Label = "Apellidos", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Email", .Label = "Email", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Telefono", .Label = "Telefono", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "FechaIngreso", .Label = "Fecha Ingreso", .Type = "date", .Required = True},
                        New CrudCampoConfig With {.Name = "SalarioBase", .Label = "Salario Base", .Type = "number", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
                    }
                }


            Case "PRODUCTO"
                config = New CrudTablaConfig With {
                    .TableName = "Producto",
                    .Pk = "ProductoId",
                    .Endpoint = "Producto",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "ProductoId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "Referencia", .Label = "Referencia", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Nombre", .Label = "Nombre", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Descripcion", .Label = "Descripcion", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Tipo", .Label = "Tipo (INTERIOR/EXTERIOR)", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Material", .Label = "Material", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "AltoCm", .Label = "Alto (cm)", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "AnchoCm", .Label = "Ancho (cm)", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "ProfundidadCm", .Label = "Profundidad (cm)", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Color", .Label = "Color", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "PesoGramos", .Label = "Peso (gramos)", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "ImagenUrl", .Label = "Imagen URL", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "UnidadMedidaId", .Label = "Unidad Medida", .Type = "number", .Nullable = True},
                        New CrudCampoConfig With {.Name = "CategoriaId", .Label = "Categoria", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "LoteProducto", .Label = "Lote Producto", .Type = "text", .Nullable = True}
                    }
                }


            Case "INVENTARIO_PRODUCTO"
                config = New CrudTablaConfig With {
                    .TableName = "Inventario_Producto",
                    .Pk = "InvProdId",
                    .Endpoint = "Inventario_Producto",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "InvProdId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "ProductoId", .Label = "Producto", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Stock", .Label = "Stock", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "StockReservado", .Label = "Stock Reservado", .Type = "number", .Required = True, .DefaultValue = "0"},
                        New CrudCampoConfig With {.Name = "StockMinimo", .Label = "Stock Minimo", .Type = "number", .Nullable = True}
                    }
                }


            Case "MOVIMIENTO_INVENTARIO"
                config = New CrudTablaConfig With {
                    .TableName = "Movimiento_Inventario",
                    .Pk = "MovInvId",
                    .Endpoint = "Movimiento_Inventario",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "MovInvId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "InvProdId", .Label = "Inventario Producto", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "TipoMov", .Label = "Tipo Movimiento", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Cantidad", .Label = "Cantidad", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Motivo", .Label = "Motivo", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "ReferenciaId", .Label = "Referencia Id", .Type = "number", .Nullable = True},
                        New CrudCampoConfig With {.Name = "MovAt", .Label = "Fecha Movimiento", .Type = "datetime-local", .Required = True}
                    }
                }


            Case "PRECIO_HISTORICO"
                config = New CrudTablaConfig With {
                    .TableName = "Precio_Historico",
                    .Pk = "PrecioHistId",
                    .Endpoint = "Precio_Historico",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "PrecioHistId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {
                            .Name = "ProductoId",
                            .Label = "Producto",
                            .Type = "select",
                            .Required = True,
                            .DataSource = "/Producto/Index",
                            .ValueField = "ProductoId",
                            .TextField = "Nombre"
                        },
                        New CrudCampoConfig With {.Name = "Precio", .Label = "Precio", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "VigenciaInicio", .Label = "Vigencia Inicio", .Type = "date", .Required = True},
                        New CrudCampoConfig With {.Name = "VigenciaFin", .Label = "Vigencia Fin", .Type = "date", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Motivo", .Label = "Motivo", .Type = "text", .Nullable = True}
                    }
                }


            Case "MATERIA_PRIMA"
                config = New CrudTablaConfig With {
                    .TableName = "Materia_Prima",
                    .Pk = "MpId",
                    .Endpoint = "Materia_Prima",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "MpId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "Codigo", .Label = "Codigo", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Nombre", .Label = "Nombre", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "UnidadMedidaId", .Label = "Unidad Medida", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Descripcion", .Label = "Descripcion", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
                    }
                }


            Case "PROVEEDOR"
                config = New CrudTablaConfig With {
                    .TableName = "Proveedor",
                    .Pk = "ProvId",
                    .Endpoint = "Proveedor",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "ProvId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "RazonSocial", .Label = "Razon Social", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Nit", .Label = "NIT", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Email", .Label = "Email", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Telefono", .Label = "Telefono", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Direccion", .Label = "Direccion", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Ciudad", .Label = "Ciudad", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Pais", .Label = "Pais", .Type = "text", .Nullable = True}
                    }
                }


            Case "ORDEN_COMPRA"
                config = New CrudTablaConfig With {
                    .TableName = "Orden_Compra",
                    .Pk = "OrdenCompraId",
                    .Endpoint = "Orden_Compra",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "OrdenCompraId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "NumOc", .Label = "Numero OC", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "ProvId", .Label = "Proveedor", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "EstadoOcId", .Label = "Estado Orden Compra", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "CondicionPagoId", .Label = "Condicion Pago", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "FechaOc", .Label = "Fecha OC", .Type = "date", .Required = True},
                        New CrudCampoConfig With {.Name = "Subtotal", .Label = "Subtotal", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Impuesto", .Label = "Impuesto", .Type = "number", .Required = True, .DefaultValue = "0"},
                        New CrudCampoConfig With {.Name = "Total", .Label = "Total", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Observaciones", .Label = "Observaciones", .Type = "text", .Nullable = True}
                    }
                }


            Case "ORDEN_COMPRA_DETALLE"
                config = New CrudTablaConfig With {
                    .TableName = "Orden_Compra_Detalle",
                    .Pk = "OrdenCompraDetId",
                    .Endpoint = "Orden_Compra_Detalle",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "OrdenCompraDetId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "OrdenCompraId", .Label = "Orden Compra", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "MpId", .Label = "Materia Prima", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Cantidad", .Label = "Cantidad", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "CostoUnitario", .Label = "Costo Unitario", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "SubtotalLinea", .Label = "Subtotal Linea", .Type = "number", .Required = True}
                    }
                }


            Case "RECEPCION_MATERIAL"
                config = New CrudTablaConfig With {
                    .TableName = "Recepcion_Material",
                    .Pk = "RecepcionMaterialId",
                    .Endpoint = "Recepcion_Material",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "RecepcionMaterialId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "OrdenCompraId", .Label = "Orden Compra", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "FechaRecepcion", .Label = "Fecha Recepcion", .Type = "date", .Required = True},
                        New CrudCampoConfig With {.Name = "EmpIdRecibe", .Label = "Empleado Recibe", .Type = "number", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Observaciones", .Label = "Observaciones", .Type = "text", .Nullable = True}
                    }
                }


            Case "CONTROL_CALIDAD"
                config = New CrudTablaConfig With {
                    .TableName = "Control_Calidad",
                    .Pk = "ControlCalidadId",
                    .Endpoint = "Control_Calidad",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "ControlCalidadId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "Origen", .Label = "Origen", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "OrdenProduccionId", .Label = "Orden Produccion", .Type = "number", .Nullable = True},
                        New CrudCampoConfig With {.Name = "RecepcionMaterialId", .Label = "Recepcion Material", .Type = "number", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Resultado", .Label = "Resultado", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Observacion", .Label = "Observacion", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "InspeccionAt", .Label = "Inspeccion", .Type = "datetime-local", .Required = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
                    }
                }


            Case "SESION"
                config = New CrudTablaConfig With {
                    .TableName = "Sesion",
                    .Pk = "SesionId",
                    .Endpoint = "Sesion",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "SesionId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "UsuId", .Label = "Usuario", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "TokenHash", .Label = "Token Hash", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Ip", .Label = "IP", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "UserAgent", .Label = "User Agent", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "InicioAt", .Label = "Inicio", .Type = "datetime-local", .Required = True},
                        New CrudCampoConfig With {.Name = "FinAt", .Label = "Fin", .Type = "datetime-local", .Nullable = True}
                    }
                }


            Case "HISTORIAL_CAMBIO_CONTRASENA"
                config = New CrudTablaConfig With {
                    .TableName = "Historial_Cambio_Contrasena",
                    .Pk = "HcId",
                    .Endpoint = "Historial_Cambio_Contrasena",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "HcId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "UsuId", .Label = "Usuario", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "CambioAt", .Label = "Cambio", .Type = "datetime-local", .Required = True},
                        New CrudCampoConfig With {.Name = "Motivo", .Label = "Motivo", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Ip", .Label = "IP", .Type = "text", .Nullable = True}
                    }
                }


            Case "VEHICULO"
                config = New CrudTablaConfig With {
                    .TableName = "Vehiculo",
                    .Pk = "VehiculoId",
                    .Endpoint = "Vehiculo",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "VehiculoId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "Placa", .Label = "Placa", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Tipo", .Label = "Tipo", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "CapacidadKg", .Label = "Capacidad Kg", .Type = "number", .Nullable = True},
                        New CrudCampoConfig With {.Name = "CapacidadM3", .Label = "Capacidad M3", .Type = "number", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Activo", .Label = "Activo (1/0)", .Type = "number", .Required = True, .DefaultValue = "1"}
                    }
                }


            Case "MOVIMIENTO_MATERIA_PRIMA"
                config = New CrudTablaConfig With {
                    .TableName = "Movimiento_Materia_Prima",
                    .Pk = "MovMpId",
                    .Endpoint = "Movimiento_Materia_Prima",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "MovMpId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "InvMpId", .Label = "Inventario Materia Prima", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "TipoMov", .Label = "Tipo Movimiento", .Type = "text", .Required = True},
                        New CrudCampoConfig With {.Name = "Cantidad", .Label = "Cantidad", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Motivo", .Label = "Motivo", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "MovAt", .Label = "Fecha Movimiento", .Type = "datetime-local", .Required = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Required = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "INVENTARIO_MATERIA_PRIMA"
                config = New CrudTablaConfig With {
                    .TableName = "Inventario_Materia_Prima",
                    .Pk = "InvMpId",
                    .Endpoint = "Inventario_Materia_Prima",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "InvMpId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "MpId", .Label = "Materia Prima Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "Stock", .Label = "Stock", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "StockMinimo", .Label = "Stock Minimo", .Type = "number", .Nullable = True},
                        New CrudCampoConfig With {.Name = "CreatedAt", .Label = "Fecha Creacion", .Type = "datetime-local", .Nullable = True},
                        New CrudCampoConfig With {.Name = "UpdatedAt", .Label = "Fecha Actualizacion", .Type = "datetime-local", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Nullable = True, .DefaultValue = "ACTIVO"}
                    }
                }

            Case "RUTA_ENTREGA"
                config = New CrudTablaConfig With {
                    .TableName = "Ruta_Entrega",
                    .Pk = "RutaEntregaId",
                    .Endpoint = "Ruta_Entrega",
                    .Campos = New List(Of CrudCampoConfig) From {
                        New CrudCampoConfig With {.Name = "RutaEntregaId", .Label = "Id", .Type = "hidden"},
                        New CrudCampoConfig With {.Name = "VehiculoId", .Label = "Vehiculo Id", .Type = "number", .Required = True},
                        New CrudCampoConfig With {.Name = "FechaRuta", .Label = "Fecha Ruta", .Type = "date", .Required = True},
                        New CrudCampoConfig With {.Name = "Descripcion", .Label = "Descripcion", .Type = "text", .Nullable = True},
                        New CrudCampoConfig With {.Name = "EstadoRuta", .Label = "Estado Ruta", .Type = "text", .Nullable = True, .DefaultValue = "PLANIFICADA"},
                        New CrudCampoConfig With {.Name = "CreatedAt", .Label = "Fecha Creacion", .Type = "datetime-local", .Nullable = True},
                        New CrudCampoConfig With {.Name = "UpdatedAt", .Label = "Fecha Actualizacion", .Type = "datetime-local", .Nullable = True},
                        New CrudCampoConfig With {.Name = "Estado", .Label = "Estado", .Type = "text", .Nullable = True, .DefaultValue = "ACTIVO"}
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
