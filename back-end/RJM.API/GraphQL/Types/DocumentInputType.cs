using GraphQL.Types;

namespace RJM.API.GraphQL.Types
{
    public class DocumentInputType : InputObjectGraphType
    {
        public DocumentInputType()
        {
            Name = "documentInput";
            Field<IdGraphType>("id");
            Field<NonNullGraphType<StringGraphType>>("name");
            Field<StringGraphType>("displayName");
            Field<StringGraphType>("description");
            Field<StringGraphType>("path");
            Field<StringGraphType>("url");
            Field<IntGraphType>("sizeInBytes");
            Field<DateTimeGraphType>("fileLastModifiedOn");
            Field<StringGraphType>("mimeType");
            Field<IdGraphType>("documentTypeId");

            // To create a link with Resume directly on create of Document.
            //Field<IdGraphType>("resumeId");
        }
    }
}
