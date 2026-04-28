Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Produccion

Namespace Servicios
    Public Class ListaMaterialesDetalleServicio

        Private ReadOnly _datos As ListaMaterialesDetalleDatos

        Public Sub New()
            _datos = New ListaMaterialesDetalleDatos()
        End Sub

        Public Function Insertar(ByVal entidad As ListaMaterialesDetalle) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Function Actualizar(ByVal entidad As ListaMaterialesDetalle) As Boolean
            Return _datos.Actualizar(entidad)
        End Function

        Public Function Eliminar(ByVal listaMaterialesDetId As Integer) As Boolean
            Return _datos.Eliminar(listaMaterialesDetId)
        End Function

        Public Function ObtenerPorId(ByVal listaMaterialesDetId As Integer) As ListaMaterialesDetalle
            Return _datos.ObtenerPorId(listaMaterialesDetId)
        End Function

        Public Function Listar() As List(Of ListaMaterialesDetalle)
            Return _datos.Listar()
        End Function

    End Class
End Namespace