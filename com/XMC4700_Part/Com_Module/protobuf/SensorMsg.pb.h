/* Automatically generated nanopb header */
/* Generated by nanopb-0.3.6 at Wed Nov 16 15:57:41 2016. */

#ifndef PB_SENSORMSG_PB_H_INCLUDED
#define PB_SENSORMSG_PB_H_INCLUDED
#include "pb.h"

/* @@protoc_insertion_point(includes) */
#if PB_PROTO_HEADER_VERSION != 30
#error Regenerate this file with the current version of nanopb generator.
#endif

#ifdef __cplusplus
extern "C" {
#endif

/* Struct definitions */
typedef struct _Com_Module_DataEntry {
    uint32_t SensorId;
    double Data;
/* @@protoc_insertion_point(struct:Com_Module_DataEntry) */
} Com_Module_DataEntry;

typedef struct _Com_Module_SensorMsg {
    uint64_t SequenceNr;
    pb_callback_t DataTable;
/* @@protoc_insertion_point(struct:Com_Module_SensorMsg) */
} Com_Module_SensorMsg;

/* Default values for struct fields */

/* Initializer values for message structs */
#define Com_Module_DataEntry_init_default        {0, 0}
#define Com_Module_SensorMsg_init_default        {0, {{NULL}, NULL}}
#define Com_Module_DataEntry_init_zero           {0, 0}
#define Com_Module_SensorMsg_init_zero           {0, {{NULL}, NULL}}

/* Field tags (for use in manual encoding/decoding) */
#define Com_Module_DataEntry_SensorId_tag        1
#define Com_Module_DataEntry_Data_tag            2
#define Com_Module_SensorMsg_SequenceNr_tag      1
#define Com_Module_SensorMsg_DataTable_tag       2

/* Struct field encoding specification for nanopb */
extern const pb_field_t Com_Module_DataEntry_fields[3];
extern const pb_field_t Com_Module_SensorMsg_fields[3];

/* Maximum encoded size of messages (where known) */
#define Com_Module_DataEntry_size                15
/* Com_Module_SensorMsg_size depends on runtime parameters */

/* Message IDs (where set with "msgid" option) */
#ifdef PB_MSGID

#define SENSORMSG_MESSAGES \


#endif

#ifdef __cplusplus
} /* extern "C" */
#endif
/* @@protoc_insertion_point(eof) */

#endif