Option Strict On
Option Explicit On

Imports System.Web.Mvc

Public Class MetadataController
    Inherits Controller

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
        End Select

        If config Is Nothing Then
            Return Json(New With {
                .success = False,
                .message = "No existe metadata para la tabla solicitada."
            }, JsonRequestBehavior.AllowGet)
        End If

        Return Json(config, JsonRequestBehavior.AllowGet)
    End Function

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
            New With {.Value = "Tipo_Promocion", .Text = "Tipo_Promocion"},
            New With {.Value = "Campana_Marketing", .Text = "Campana_Marketing"},
            New With {.Value = "Cupon", .Text = "Cupon"},
            New With {.Value = "FAQ", .Text = "FAQ"}
        }

        Return Json(tablas, JsonRequestBehavior.AllowGet)
    End Function

End Class