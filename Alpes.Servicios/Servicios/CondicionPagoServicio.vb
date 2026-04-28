Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Compras

Namespace Servicios
    Public Class CondicionPagoServicio

        Private ReadOnly _datos As CondicionPagoDatos

        Public Sub New()
            _datos = New CondicionPagoDatos()
        End Sub

        Public Function Insertar(ByVal entidad As CondicionPago) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Function Actualizar(ByVal entidad As CondicionPago) As Boolean
            Return _datos.Actualizar(entidad)
        End Function

        Public Function Eliminar(ByVal condicionPagoId As Integer) As Boolean
            Return _datos.Eliminar(condicionPagoId)
        End Function

        Public Function ObtenerPorId(ByVal condicionPagoId As Integer) As CondicionPago
            Return _datos.ObtenerPorId(condicionPagoId)
        End Function

        Public Function Listar() As List(Of CondicionPago)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of CondicionPago)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace