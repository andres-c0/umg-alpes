Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Produccion

Namespace Servicios
    Public Class ListaMaterialesServicio

        Private ReadOnly _datos As ListaMaterialesDatos

        Public Sub New()
            _datos = New ListaMaterialesDatos()
        End Sub

        Public Function Insertar(ByVal entidad As ListaMateriales) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Function Actualizar(ByVal entidad As ListaMateriales) As Boolean
            Return _datos.Actualizar(entidad)
        End Function

        Public Function Eliminar(ByVal listaMaterialesId As Integer) As Boolean
            Return _datos.Eliminar(listaMaterialesId)
        End Function

        Public Function ObtenerPorId(ByVal listaMaterialesId As Integer) As ListaMateriales
            Return _datos.ObtenerPorId(listaMaterialesId)
        End Function

        Public Function Listar() As List(Of ListaMateriales)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal valor As String) As List(Of ListaMateriales)
            Return _datos.Buscar(valor)
        End Function

    End Class
End Namespace