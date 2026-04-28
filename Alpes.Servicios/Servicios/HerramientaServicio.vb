Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Produccion

Namespace Servicios
    Public Class HerramientaServicio

        Private ReadOnly _datos As HerramientaDatos

        Public Sub New()
            _datos = New HerramientaDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Herramienta) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Function Actualizar(ByVal entidad As Herramienta) As Boolean
            Return _datos.Actualizar(entidad)
        End Function

        Public Function Eliminar(ByVal herramientaId As Integer) As Boolean
            Return _datos.Eliminar(herramientaId)
        End Function

        Public Function ObtenerPorId(ByVal herramientaId As Integer) As Herramienta
            Return _datos.ObtenerPorId(herramientaId)
        End Function

        Public Function Listar() As List(Of Herramienta)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal valor As String) As List(Of Herramienta)
            Return _datos.Buscar(valor)
        End Function

    End Class
End Namespace