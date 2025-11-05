namespace ClientsService.Src.Helpers
{
    /// <summary>
    /// Clase genérica para estructurar las respuestas de la API.
    /// </summary>
    /// <typeparam name="T">Tipo de datos que contiene la respuesta.</typeparam>
    /// <param name="success">Indica si la operación fue exitosa.</param>
    /// <param name="message">Mensaje descriptivo de la respuesta.</param>
    /// <param name="data">Datos devueltos por la operación.</param>
    /// <param name="errors">Lista de errores, si los hay.</param>
    public class ApiResponse<T>(
        bool success,
        string message,
        T? data = default,
        List<string>? errors = null
    )
    {
        /// <summary>
        /// Indica si la operación fue exitosa.
        /// </summary>
        public bool Success { get; set; } = success;

        /// <summary>
        /// Mensaje descriptivo de la respuesta.
        /// </summary>
        public string Message { get; set; } = message;

        /// <summary>
        /// Datos devueltos por la operación.
        /// </summary>
        public T? Data { get; set; } = data;

        /// <summary>
        /// Lista de errores, si los hay.
        /// </summary>
        public List<string>? Errors { get; set; } = errors;
    }
}
