package org.healthwise.solr

import org.springframework.boot.autoconfigure.SpringBootApplication
import org.springframework.boot.runApplication

@SpringBootApplication
class SolrApplication

fun main(args: Array<String>) {
	runApplication<SolrApplication>(*args)
}
