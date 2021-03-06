using GraphQL.Types;
using RJM.API.DAL.Repositories;
using RJM.API.Models;
using System;

namespace RJM.API.GraphQL.Types
{
    public class DocumentType : ObjectGraphType<Document>
    {
        public DocumentType(
            DocumentTypeRepository documentTypeRepository,
            DocumentRepository documentRepository,
            ResumeRepository resumeRepository,
            DocumentResumeRepository documentResumeRepository
        )
        {
            Field(x => x.Id, type: typeof(IdGraphType));
            Field(x => x.Name);
            Field(x => x.DisplayName, nullable: true);
            Field(x => x.Description, nullable: true);
            Field(x => x.Path, nullable: true);
            Field(x => x.URL, nullable: true);
            Field(x => x.SizeInBytes, nullable: true);
            Field(x => x.FileLastModifiedOn, nullable: true);
            Field(x => x.MimeType, nullable: true);

            Field<DocumentTypeType>(
                "documentType",
                resolve: context =>
                {
                    if (context.Source.DocumentTypeId != null)
                        return documentTypeRepository.GetById((Guid)context.Source.DocumentTypeId);
                    return null;
                }
            );

            //// Async test
            //FieldAsync<DocumentTypeType>(
            //    "documentType",
            //    resolve: async context =>
            //    {
            //        if (context.Source.DocumentTypeId != null) {
            //            return await context.TryAsyncResolve(
            //                async c => await documentTypeRepository.GetByIdAsync((Guid)context.Source.DocumentTypeId)
            //            );
            //        }
            //        
            //        return null;
            //    }
            //);

            Field<ListGraphType<ResumeType>>(
                "resumes",
                resolve: context => resumeRepository.GetByDocumentId(context.Source.Id)
            );

            //// Async test
            //FieldAsync<ListGraphType<ResumeType>>(
            //    "resumes",
            //    resolve: async context =>
            //    {
            //        return await context.TryAsyncResolve(
            //            async c => await resumeRepository.GetByDocumentIdAsync(context.Source.Id)
            //        );
            //    }
            //);

            Field(x => x.CreatedByUserId, type: typeof(IdGraphType));
            // TODO: Field(x => x.CreatedByUser, type: typeof(UserType));
            Field(x => x.ModifiedByUserId, type: typeof(IdGraphType));
            // TODO: Field(x => x.ModifiedByUser, type: typeof(UserType));
        }
    }
}
