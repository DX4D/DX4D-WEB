<?php
/**
 * Functions.php
 *
 * @package  Theme_Customisations
 * @author   WooThemes
 * @since    1.0.0
 */

if ( ! defined( 'ABSPATH' ) ) {
	exit; // Exit if accessed directly.
}

/**
 * functions.php
 * Add PHP snippets here
 */

/**
 * @snippet       Remove Grouped Product Price Range
 * @how-to        Watch tutorial @ https://businessbloomer.com/?p=19055
 * @sourcecode    https://businessbloomer.com/?p=22191
 * @author        Rodolfo Melogli
 * @compatible    WooCommerce 3.0.4
 */
 
add_filter( 'woocommerce_grouped_price_html', 'bbloomer_grouped_price_range_delete', 10, 3 );
 
function bbloomer_grouped_price_range_delete( $price, $product, $child_prices ) {
$price = '';
return $price;
}