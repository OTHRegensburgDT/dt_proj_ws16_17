/* Automatically generated nanopb header */
/* Generated by nanopb-0.3.6 at Wed Nov 30 14:29:48 2016. */

#ifndef PB_PARAMMSG_PB_H_INCLUDED
#define PB_PARAMMSG_PB_H_INCLUDED
#include "pb.h"

/* @@protoc_insertion_point(includes) */
#if PB_PROTO_HEADER_VERSION != 30
#error Regenerate this file with the current version of nanopb generator.
#endif

#ifdef __cplusplus
extern "C" {
#endif

/* Struct definitions */
typedef struct _Regulation_RegParams {
    uint32_t target;
    float paraP;
    float paraI;
    float paraD;
    float tgtVal;
/* @@protoc_insertion_point(struct:Regulation_RegParams) */
} Regulation_RegParams;

/* Default values for struct fields */

/* Initializer values for message structs */
#define Regulation_RegParams_init_default        {0, 0, 0, 0, 0}
#define Regulation_RegParams_init_zero           {0, 0, 0, 0, 0}

/* Field tags (for use in manual encoding/decoding) */
#define Regulation_RegParams_target_tag          1
#define Regulation_RegParams_paraP_tag           2
#define Regulation_RegParams_paraI_tag           3
#define Regulation_RegParams_paraD_tag           4
#define Regulation_RegParams_tgtVal_tag          5

/* Struct field encoding specification for nanopb */
extern const pb_field_t Regulation_RegParams_fields[6];

/* Maximum encoded size of messages (where known) */
#define Regulation_RegParams_size                26

/* Message IDs (where set with "msgid" option) */
#ifdef PB_MSGID

#define PARAMMSG_MESSAGES \


#endif

#ifdef __cplusplus
} /* extern "C" */
#endif
/* @@protoc_insertion_point(eof) */

#endif
