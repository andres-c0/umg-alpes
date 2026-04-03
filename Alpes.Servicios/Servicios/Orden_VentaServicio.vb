Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Ventas

Namespace Servicios
    Public Class Orden_VentaServicio

        Private ReadOnly _datos As Orden_VentaDatos

        Public Sub New()
            _datos = New Orden_VentaDatos()
        End Sub

        Public Function Listar() As List(Of Orden_Venta)
            Return _datos.Listar()
        End Function

        Public Function ObtenerPorId(id As Integer) As Orden_Venta
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Buscar(valor As String) As List(Of Orden_Venta)
            Return _datos.Buscar(valor)
        End Function

        Public Function Insertar(entidad As Orden_Venta) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(entidad As Orden_Venta)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(id As Integer)
            _datos.Eliminar(id)
        End Sub

    End Class
End Namespace