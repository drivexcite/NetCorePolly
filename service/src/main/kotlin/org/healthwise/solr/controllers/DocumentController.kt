package org.healthwise.solr.controllers

import kotlinx.coroutines.delay
import org.healthwise.solr.dtos.LegacyDocument
import org.healthwise.solr.dtos.LegacyDocumentResponse
import org.springframework.http.ResponseEntity
import org.springframework.web.bind.annotation.GetMapping
import org.springframework.web.bind.annotation.RequestParam
import org.springframework.web.bind.annotation.RestController
import java.util.concurrent.atomic.AtomicInteger
import java.util.concurrent.atomic.AtomicLong

@RestController
class DocumentsController {
    @GetMapping("/documents")
    suspend fun getDocuments(@RequestParam(required = false, defaultValue = "0") skip: Int = 0, @RequestParam(required = false, defaultValue = "100") top: Int = 100) : ResponseEntity<LegacyDocumentResponse>  {
        delay(60)

        val items = (1..top).map {
            LegacyDocument(
                    hwid = "abn-666-${skip + it}",
                    title = "Super Document ${skip + it}",
                    rank = "${it % 5}"
            )
        }.toList()

        return ResponseEntity.ok(LegacyDocumentResponse(items, skip, top, 20_000))
    }
}