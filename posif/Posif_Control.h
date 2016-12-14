#ifndef POSIF_CONTROL_H
#define POSIF_CONTROL_H

/* 
 * Needed input:
 *  Function Selector -> PCONF.FSEL = 11, Multi-Channel & Quadrature Decoder
 *      POSIF0.IN0[D...A] ... POSIF0.IN2[D...A] 
 *      POSIF0.OUT[6...0]
        POSIFx.OUT6 Pin auf High -> Update shadow request
 *
 *  Hall Sensor Control:
 *      POSIF0.HSD[B...A]
 *      POSIF0.EWHE[D...A]
 *  
 *  Multi channel mode:
 *      Input:
 *          POSIF0.MSET[H...A] - Next pattern input
 *          POSIF0.MSYNC[D...A] - Synchro to pwm - shadow vars get updated on falling edge
 *      Output:
 *          POSIF0.MOUT[15:0]
 *      MCSS ist output
 *      MCM.MCMP ist verbunden mit MOUT
 *      CC4/8yTC.MCME = 1
 */

/* Andi verwendet POSIF1 */
#define POSIF_PTR               POSIF0
#define CCU40_MODULE_PTR        CCU40
#define CCU40_MODULE_NUMBER     (0U)
#define CCU40_SLICE0_PTR        CCU40_CC40
#define CCU40_SLICE0_NUMBER     (0U)
#define CCU40_SLICE0_OUTPUT     P1_0

/* CCU8 Macros */
#define CCU80_MODULE_PTR        CCU80
#define CCU80_MODULE_NUMBER     (0U)

#define CCU80_SLICE0_PTR        CCU80_CC80
#define CCU80_SLICE0_NUMBER     (0U)
#define CCU80_SLICE0_OUTPUT00   P0_0
#define CCU80_SLICE0_OUTPUT01   P0_1
#define CCU80_SLICE0_OUTPUT02   P0_2
#define CCU80_SLICE0_OUTPUT03   P0_3

#define CCU80_SLICE1_PTR        CCU80_CC81
#define CCU80_SLICE1_NUMBER     (1U)
#define CCU80_SLICE1_OUTPUT10   P0_7
#define CCU80_SLICE1_OUTPUT11   P0_6
#define CCU80_SLICE1_OUTPUT12   P0_5
#define CCU80_SLICE1_OUTPUT13   P0_4

void POSIF0_0_IRQHandler(void);

#endif
