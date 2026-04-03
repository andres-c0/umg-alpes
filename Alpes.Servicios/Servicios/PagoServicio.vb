
Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Ventas

Namespace Servicios
    Public Class PagoServicio

        Private ReadOnly _datos As PagoDatos

        Public Sub New()
            _datos = New PagoDatos()
        End Sub

        Public Function Listar() As List(Of Pago)
            Return _datos.Listar()
        End Function

        Public Function ObtenerPorId(ByVal id As Integer) As Pago
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Buscar(ByVal valor As Decimal) As List(Of Pago)
            Return _datos.Buscar(valor)
        End Function

        Public Function Insertar(ByVal entidad As Pago) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Pago)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

    End Class
End Namespace