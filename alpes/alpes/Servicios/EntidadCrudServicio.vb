Imports System.Data
Imports System.Reflection
Imports Newtonsoft.Json

Namespace Servicios
    Public Class EntidadCrudServicio

        Private Shared ReadOnly _mapaEntidades As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase) From {
            {"CLIENTE", "ClienteDatos"},
            {"ESTADO_ENVIO", "EstadoEnvioDatos"},
            {"TIPO_ENTREGA", "TipoEntregaDatos"},
            {"ZONA_ENVIO", "ZonaEnvioDatos"},
            {"POLITICA_ENVIO", "PoliticaEnvioDatos"},
            {"REGLA_ENVIO_GRATIS", "ReglaEnvioGratisDatos"},
            {"TARIFA_ENVIO", "TarifaEnvioDatos"},
            {"TIPO_PROMOCION", "TipoPromocionDatos"},
            {"PROMOCION", "PromocionDatos"},
            {"PROMOCION_PRODUCTO", "PromocionProductoDatos"},
            {"REGLA_PROMOCION", "ReglaPromocionDatos"},
            {"CAMPANA_MARKETING", "CampanaMarketingDatos"},
            {"CUPON", "CuponDatos"}
        }

        Private Shared Function NormalizarNombre(nombre As String) As String
            If String.IsNullOrWhiteSpace(nombre) Then
                Return String.Empty
            End If

            Return New String(nombre.Where(Function(c) Char.IsLetterOrDigit(c)).ToArray()).ToUpperInvariant()
        End Function

        Private Shared Function ResolverTipoDatos(entidad As String) As Type
            Dim clave = NormalizarNombre(entidad)
            If String.IsNullOrWhiteSpace(clave) Then
                Return Nothing
            End If

            Dim nombreClase As String = Nothing
            If Not _mapaEntidades.TryGetValue(clave, nombreClase) Then
                Return Nothing
            End If

            Dim ensamblado = GetType(EntidadCrudServicio).Assembly
            Return ensamblado.GetTypes().FirstOrDefault(
                Function(t) t.IsClass AndAlso String.Equals(t.Name, nombreClase, StringComparison.OrdinalIgnoreCase))
        End Function

        Private Shared Function CrearInstanciaDatos(tipoDatos As Type) As Object
            Return Activator.CreateInstance(tipoDatos)
        End Function

        Private Shared Function BuscarMetodo(tipo As Type, nombre As String, cantidadParametros As Integer) As MethodInfo
            Return tipo.GetMethods().FirstOrDefault(
                Function(m) String.Equals(m.Name, nombre, StringComparison.OrdinalIgnoreCase) AndAlso m.GetParameters().Length = cantidadParametros)
        End Function

        Private Shared Function BuscarMetodoObtener(tipo As Type) As MethodInfo
            Return tipo.GetMethods().FirstOrDefault(
                Function(m)
                    If Not m.Name.StartsWith("Obtener", StringComparison.OrdinalIgnoreCase) Then Return False
                    Dim p = m.GetParameters()
                    Return p.Length = 1 AndAlso p(0).ParameterType Is GetType(Integer)
                End Function)
        End Function

        Private Shared Sub ValidarEntidadSoportada(tipoDatos As Type, entidad As String)
            If tipoDatos Is Nothing Then
                Throw New ArgumentException($"La entidad '{entidad}' no está soportada por los scripts SQL actuales.")
            End If
        End Sub

        Public Function Listar(entidad As String) As DataTable
            Dim tipoDatos = ResolverTipoDatos(entidad)
            ValidarEntidadSoportada(tipoDatos, entidad)

            Dim metodo = BuscarMetodo(tipoDatos, "Listar", 0)
            If metodo Is Nothing Then Throw New MissingMethodException(tipoDatos.FullName, "Listar")

            Dim instancia = CrearInstanciaDatos(tipoDatos)
            Return CType(metodo.Invoke(instancia, Nothing), DataTable)
        End Function

        Public Function ObtenerPorId(entidad As String, id As Integer) As DataTable
            Dim tipoDatos = ResolverTipoDatos(entidad)
            ValidarEntidadSoportada(tipoDatos, entidad)

            Dim metodo = BuscarMetodo(tipoDatos, "ObtenerPorId", 1)
            If metodo Is Nothing Then metodo = BuscarMetodoObtener(tipoDatos)
            If metodo Is Nothing Then Throw New MissingMethodException(tipoDatos.FullName, "ObtenerPorId")

            Dim instancia = CrearInstanciaDatos(tipoDatos)
            Return CType(metodo.Invoke(instancia, New Object() {id}), DataTable)
        End Function

        Public Function Buscar(entidad As String, criterio As String, valor As String) As DataTable
            Dim tipoDatos = ResolverTipoDatos(entidad)
            ValidarEntidadSoportada(tipoDatos, entidad)

            Dim metodo = BuscarMetodo(tipoDatos, "Buscar", 2)
            If metodo Is Nothing Then Throw New MissingMethodException(tipoDatos.FullName, "Buscar")

            Dim instancia = CrearInstanciaDatos(tipoDatos)
            Return CType(metodo.Invoke(instancia, New Object() {criterio, valor}), DataTable)
        End Function

        Public Function Insertar(entidad As String, payloadJson As String) As Object
            Dim tipoDatos = ResolverTipoDatos(entidad)
            ValidarEntidadSoportada(tipoDatos, entidad)

            Dim metodo = BuscarMetodo(tipoDatos, "Insertar", 1)
            If metodo Is Nothing Then Throw New MissingMethodException(tipoDatos.FullName, "Insertar")

            Dim tipoEntidad = metodo.GetParameters()(0).ParameterType
            Dim instanciaEntidad = JsonConvert.DeserializeObject(payloadJson, tipoEntidad)
            Dim instancia = CrearInstanciaDatos(tipoDatos)

            Return metodo.Invoke(instancia, New Object() {instanciaEntidad})
        End Function

        Public Sub Actualizar(entidad As String, payloadJson As String)
            Dim tipoDatos = ResolverTipoDatos(entidad)
            ValidarEntidadSoportada(tipoDatos, entidad)

            Dim metodo = BuscarMetodo(tipoDatos, "Actualizar", 1)
            If metodo Is Nothing Then Throw New MissingMethodException(tipoDatos.FullName, "Actualizar")

            Dim tipoEntidad = metodo.GetParameters()(0).ParameterType
            Dim instanciaEntidad = JsonConvert.DeserializeObject(payloadJson, tipoEntidad)
            Dim instancia = CrearInstanciaDatos(tipoDatos)

            metodo.Invoke(instancia, New Object() {instanciaEntidad})
        End Sub

        Public Sub Eliminar(entidad As String, id As Integer)
            Dim tipoDatos = ResolverTipoDatos(entidad)
            ValidarEntidadSoportada(tipoDatos, entidad)

            Dim metodo = BuscarMetodo(tipoDatos, "Eliminar", 1)
            If metodo Is Nothing Then Throw New MissingMethodException(tipoDatos.FullName, "Eliminar")

            Dim instancia = CrearInstanciaDatos(tipoDatos)
            metodo.Invoke(instancia, New Object() {id})
        End Sub

        Public Function EntidadesSoportadas() As IEnumerable(Of String)
            Return _mapaEntidades.Keys.OrderBy(Function(x) x).ToArray()
        End Function

    End Class
End Namespace
