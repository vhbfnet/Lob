namespace Lob.Core
{
    /// <summary>
    /// Offre un moyen d'invalider un objet
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// Valide l'objet
        /// </summary>
        void Validate();
    }
}
