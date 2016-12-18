/* Automatically generated nanopb constant definitions */
/* Generated by nanopb-0.3.6 at Wed Nov 16 15:57:41 2016. */

#include "SensorMsg.pb.h"

/* @@protoc_insertion_point(includes) */
#if PB_PROTO_HEADER_VERSION != 30
#error Regenerate this file with the current version of nanopb generator.
#endif



const pb_field_t Regulation_DataEntry_fields[3] = {
    PB_FIELD(  1, UINT32  , REQUIRED, STATIC  , FIRST, Regulation_DataEntry, SensorId, SensorId, 0),
    PB_FIELD(  2, DOUBLE  , REQUIRED, STATIC  , OTHER, Regulation_DataEntry, Data, SensorId, 0),
    PB_LAST_FIELD
};

const pb_field_t Regulation_SensorMsg_fields[3] = {
    PB_FIELD(  1, UINT64  , REQUIRED, STATIC  , FIRST, Regulation_SensorMsg, SequenceNr, SequenceNr, 0),
    PB_FIELD(  2, MESSAGE , REPEATED, CALLBACK, OTHER, Regulation_SensorMsg, DataTable, SequenceNr, &Regulation_DataEntry_fields),
    PB_LAST_FIELD
};


/* Check that field information fits in pb_field_t */
#if !defined(PB_FIELD_32BIT)
/* If you get an error here, it means that you need to define PB_FIELD_32BIT
 * compile-time option. You can do that in pb.h or on compiler command line.
 * 
 * The reason you need to do this is that some of your messages contain tag
 * numbers or field sizes that are larger than what can fit in 8 or 16 bit
 * field descriptors.
 */
PB_STATIC_ASSERT((pb_membersize(Regulation_SensorMsg, DataTable) < 65536), YOU_MUST_DEFINE_PB_FIELD_32BIT_FOR_MESSAGES_Regulation_DataEntry_Regulation_SensorMsg)
#endif

#if !defined(PB_FIELD_16BIT) && !defined(PB_FIELD_32BIT)
/* If you get an error here, it means that you need to define PB_FIELD_16BIT
 * compile-time option. You can do that in pb.h or on compiler command line.
 * 
 * The reason you need to do this is that some of your messages contain tag
 * numbers or field sizes that are larger than what can fit in the default
 * 8 bit descriptors.
 */
PB_STATIC_ASSERT((pb_membersize(Regulation_SensorMsg, DataTable) < 256), YOU_MUST_DEFINE_PB_FIELD_16BIT_FOR_MESSAGES_Regulation_DataEntry_Regulation_SensorMsg)
#endif


/* On some platforms (such as AVR), double is really float.
 * These are not directly supported by nanopb, but see example_avr_double.
 * To get rid of this error, remove any double fields from your .proto.
 */
PB_STATIC_ASSERT(sizeof(double) == 8, DOUBLE_MUST_BE_8_BYTES)

/* @@protoc_insertion_point(eof) */