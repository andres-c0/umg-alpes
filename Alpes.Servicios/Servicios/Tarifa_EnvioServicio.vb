Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Ventas

Namespace Servicios
    Public Class TarifaEnvioServicio

        Private ReadOnly _datos As TarifaEnvioDatos

        Public Sub New()
            _datos = New TarifaEnvioDatos()
        End Sub

        Public Function Insertar(ByVal entidad As TarifaEnvio) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As TarifaEnvio)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As TarifaEnvio
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of TarifaEnvio)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of TarifaEnvio)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace