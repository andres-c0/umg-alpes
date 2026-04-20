Option Strict On
Option Explicit On

Imports System.Linq
Imports System.Web.Mvc
Imports Alpes.Entidades
Imports Alpes.Entidades.Clientes
Imports Alpes.Servicios.Servicios

Namespace Controllers
    Public Class ClienteController
        Inherits Controller

        Private ReadOnly _servicio As ClienteServicio

        Public Sub New()
            _servicio = New ClienteServicio()
        End Sub

        ' =========================
        ' VISTAS
        ' =========================
        Function Index() As ActionResult
            Return View()
        End Function

        Function Catalogo() As ActionResult
            Return View()
        End Function

        Function Carrito() As ActionResult
            Return View()
        End Function

        Function Favoritos() As ActionResult
            Return View()
        End Function

        Function Perfil() As ActionResult
            Return View()
        End Function

        Function Pedidos() As ActionResult
            Return View()
        End Function

        ' =========================
        ' JSON PARA ADMIN
        ' =========================
        <HttpGet>
        Public Function ListarJson() As ActionResult
            Try
                Dim lista = _servicio.Listar()

                Dim resultado = lista.Select(Function(c) New With {
                    .CliId = c.CliId,
                    .TipoDocumento = c.TipoDocumento,
                    .NumDocumento = c.NumDocumento,
                    .Nombres = c.Nombres,
                    .Apellidos = c.Apellidos,
                    .NombreCompleto = ((If(c.Nombres, "") & " " & If(c.Apellidos, "")).Trim()),
                    .Email = c.Email,
                    .Direccion = c.Direccion,
                    .Ciudad = c.Ciudad,
                    .Departamento = c.Departamento,
                    .Pais = c.Pais,
                    .Estado = c.Estado
                }).ToList()

                Return Json(resultado, JsonRequestBehavior.AllowGet)
            Catch ex As Exception
                Response.StatusCode = 500
                Return Json(New With {
                    .success = False,
                    .message = ex.Message
                }, JsonRequestBehavior.AllowGet)
            End Try
        End Function

        <HttpGet>
        Public Function Obtener(ByVal id As Integer) As ActionResult
            Try
                Dim c = _servicio.ObtenerPorId(id)

                If c Is Nothing OrElse c.CliId <= 0 Then
                    Response.StatusCode = 404
                    Return Json(New With {
                        .success = False,
                        .message = "Cliente no encontrado."
                    }, JsonRequestBehavior.AllowGet)
                End If

                Return Json(New With {
                    .CliId = c.CliId,
                    .TipoDocumento = c.TipoDocumento,
                    .NumDocumento = c.NumDocumento,
                    .Nombres = c.Nombres,
                    .Apellidos = c.Apellidos,
                    .Email = c.Email,
                    .Direccion = c.Direccion,
                    .Ciudad = c.Ciudad,
                    .Departamento = c.Departamento,
                    .Pais = c.Pais,
                    .Estado = c.Estado
                }, JsonRequestBehavior.AllowGet)
            Catch ex As Exception
                Response.StatusCode = 500
                Return Json(New With {
                    .success = False,
                    .message = ex.Message
                }, JsonRequestBehavior.AllowGet)
            End Try
        End Function

        <HttpPost>
        Public Function Insertar(ByVal entidad As Cliente) As ActionResult
            Try
                If entidad Is Nothing Then
                    Response.StatusCode = 400
                    Return Json(New With {
                        .success = False,
                        .message = "Datos inválidos."
                    })
                End If

                If String.IsNullOrWhiteSpace(entidad.Nombres) OrElse String.IsNullOrWhiteSpace(entidad.Apellidos) Then
                    Response.StatusCode = 400
                    Return Json(New With {
                        .success = False,
                        .message = "Nombres y apellidos son obligatorios."
                    })
                End If

                If String.IsNullOrWhiteSpace(entidad.Email) Then
                    Response.StatusCode = 400
                    Return Json(New With {
                        .success = False,
                        .message = "El email es obligatorio."
                    })
                End If

                Dim nuevoId As Integer = _servicio.Insertar(entidad)

                Return Json(New With {
                    .success = True,
                    .message = "Cliente creado correctamente.",
                    .CliId = nuevoId
                })
            Catch ex As Exception
                Response.StatusCode = 500
                Return Json(New With {
                    .success = False,
                    .message = ex.Message
                })
            End Try
        End Function

        <HttpPost>
        Public Function Actualizar(ByVal entidad As Cliente) As ActionResult
            Try
                If entidad Is Nothing OrElse entidad.CliId <= 0 Then
                    Response.StatusCode = 400
                    Return Json(New With {
                        .success = False,
                        .message = "Cliente inválido."
                    })
                End If

                If String.IsNullOrWhiteSpace(entidad.Nombres) OrElse String.IsNullOrWhiteSpace(entidad.Apellidos) Then
                    Response.StatusCode = 400
                    Return Json(New With {
                        .success = False,
                        .message = "Nombres y apellidos son obligatorios."
                    })
                End If

                If String.IsNullOrWhiteSpace(entidad.Email) Then
                    Response.StatusCode = 400
                    Return Json(New With {
                        .success = False,
                        .message = "El email es obligatorio."
                    })
                End If

                _servicio.Actualizar(entidad)

                Return Json(New With {
                    .success = True,
                    .message = "Cliente actualizado correctamente."
                })
            Catch ex As Exception
                Response.StatusCode = 500
                Return Json(New With {
                    .success = False,
                    .message = ex.Message
                })
            End Try
        End Function

        <HttpPost>
        Public Function Eliminar(ByVal CliId As Integer) As ActionResult
            Try
                If CliId <= 0 Then
                    Response.StatusCode = 400
                    Return Json(New With {
                        .success = False,
                        .message = "Id inválido."
                    })
                End If

                _servicio.Eliminar(CliId)

                Return Json(New With {
                    .success = True,
                    .message = "Cliente eliminado correctamente."
                })
            Catch ex As Exception
                Response.StatusCode = 500
                Return Json(New With {
                    .success = False,
                    .message = ex.Message
                })
            End Try
        End Function

        <HttpGet>
        Public Function Buscar(ByVal valor As String) As ActionResult
            Try
                Dim lista = _servicio.Buscar("GENERAL", valor)

                Dim resultado = lista.Select(Function(c) New With {
                    .CliId = c.CliId,
                    .TipoDocumento = c.TipoDocumento,
                    .NumDocumento = c.NumDocumento,
                    .Nombres = c.Nombres,
                    .Apellidos = c.Apellidos,
                    .NombreCompleto = ((If(c.Nombres, "") & " " & If(c.Apellidos, "")).Trim()),
                    .Email = c.Email,
                    .Direccion = c.Direccion,
                    .Ciudad = c.Ciudad,
                    .Departamento = c.Departamento,
                    .Pais = c.Pais,
                    .Estado = c.Estado
                }).ToList()

                Return Json(resultado, JsonRequestBehavior.AllowGet)
            Catch ex As Exception
                Response.StatusCode = 500
                Return Json(New With {
                    .success = False,
                    .message = ex.Message
                }, JsonRequestBehavior.AllowGet)
            End Try
        End Function

    End Class
End Namespace