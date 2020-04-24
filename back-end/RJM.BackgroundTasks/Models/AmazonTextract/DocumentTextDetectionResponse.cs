using System;
using System.Collections.Generic;
using System.Text;

namespace RJM.BackgroundTasks.Models.AmazonTextract
{
    public class DocumentTextDetectionResponse
    {
        public DocumentTextDetectionResponseBlock[] Blocks { get; set; }
        public string DetectDocumentTextModelVersion { get; set; }
        public DocumentTextDetectionResponseDocumentMetadata DocumentMetadata { get; set; }
        public DocumentTextDetectionResponseJobStatus JobStatus { get; set; }
        public object NextToken { get; set; }
        public object StatusMessage { get; set; }
        public object[] Warnings { get; set; }
        public DocumentTextDetectionResponseResponseMetadata ResponseMetadata { get; set; }
        public int ContentLength { get; set; }
        public int HttpStatusCode { get; set; }
    }

    public class DocumentTextDetectionResponseDocumentMetadata
    {
        public int Pages { get; set; }
    }

    public class DocumentTextDetectionResponseJobStatus
    {
        public string Value { get; set; }
    }

    public class DocumentTextDetectionResponseResponseMetadata
    {
        public string RequestId { get; set; }
        public DocumentTextDetectionResponseMetadata Metadata { get; set; }
    }

    public class DocumentTextDetectionResponseMetadata
    {
    }

    public class DocumentTextDetectionResponseBlock
    {
        public DocumentTextDetectionResponseBlockType BlockType { get; set; }
        public int ColumnIndex { get; set; }
        public int ColumnSpan { get; set; }
        public float Confidence { get; set; }
        public object[] EntityTypes { get; set; }
        public DocumentTextDetectionResponseGeometry Geometry { get; set; }
        public string Id { get; set; }
        public int Page { get; set; }
        public DocumentTextDetectionResponseRelationship[] Relationships { get; set; }
        public int RowIndex { get; set; }
        public int RowSpan { get; set; }
        public object SelectionStatus { get; set; }
        public string Text { get; set; }
    }

    public class DocumentTextDetectionResponseBlockType
    {
        public string Value { get; set; }
    }

    public class DocumentTextDetectionResponseGeometry
    {
        public DocumentTextDetectionResponseBoundingbox BoundingBox { get; set; }
        public DocumentTextDetectionResponsePolygon[] Polygon { get; set; }
    }

    public class DocumentTextDetectionResponseBoundingbox
    {
        public float Height { get; set; }
        public float Left { get; set; }
        public float Top { get; set; }
        public float Width { get; set; }
    }

    public class DocumentTextDetectionResponsePolygon
    {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class DocumentTextDetectionResponseRelationship
    {
        public string[] Ids { get; set; }
        public DocumentTextDetectionResponseType Type { get; set; }
    }

    public class DocumentTextDetectionResponseType
    {
        public string Value { get; set; }
    }
}
