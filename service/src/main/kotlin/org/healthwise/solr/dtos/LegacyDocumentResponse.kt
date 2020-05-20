package org.healthwise.solr.dtos

data class LegacyDocumentResponse(val items: List<LegacyDocument>, val skip: Int, val top : Int, val available: Int)