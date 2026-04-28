Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Envios

Namespace Servicios
    Public Class Tipo_EntregaServicio

        Private ReadOnly _datos As Tipo_EntregaDatos

        Public Sub New()
            _datos = New Tipo_EntregaDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Tipo_Entrega) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Tipo_Entrega)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Tipo_Entrega
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Tipo_Entrega)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Tipo_Entrega)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace