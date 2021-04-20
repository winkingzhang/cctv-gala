namespace Thoughtworks.Gala.WebApi.ValueObjects
{
    /// <summary>
    /// Represent the general request model, in which wraps a generic data with reference type
    /// </summary>
    /// <typeparam name="TViewModel">generic data type, must be a reference type</typeparam>
    public class Request<TViewModel> where TViewModel: class
    {
        /// <summary>
        /// Represent the data in this request, it can be nullable
        /// </summary>
        public TViewModel? Data { get; set; }
    }
}
