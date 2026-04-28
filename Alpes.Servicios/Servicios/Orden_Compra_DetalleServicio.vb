Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Compras

Namespace Servicios
    Public Class Orden_Compra_DetalleServicio

        Private ReadOnly _datos As Orden_Compra_DetalleDatos

        Public Sub New()
            _datos = New Orden_Compra_DetalleDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Orden_Compra_Detalle) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Orden_Compra_Detalle)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Orden_Compra_Detalle
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Orden_Compra_Detalle)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Orden_Compra_Detalle)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace