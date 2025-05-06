namespace OnlineStore.Catalog.Contracts.Helpers
{
    public static class ExceptionMessagesHelper
    {
        /// <summary>
        /// Возвращает строку в формате: <b>$"Category with id {<paramref name="categoryId"/>} not found."</b>.
        /// </summary>
        public static string CategoryNotFound(int categoryId) => $"Category with ID {categoryId} not found.";

        /// <summary>
        /// Возвращает строку в формате: <b>$"<typeparamref name="T"/> with id {<paramref name="entityId"/>} not found."</b>.
        /// </summary>
        /// <typeparam name="T">Тип сущности.</typeparam>
        public static string EntityNotFound<T>(int entityId) => $"{typeof(T).Name} with ID {entityId} not found.";
    }
}
