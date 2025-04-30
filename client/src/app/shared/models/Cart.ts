import { nanoid } from 'nanoid';
export type CartType = {
    id: string;
    items: CartItem[]; 
}

export type CartItem = {
    productId: number;
    productName: string;
    price: number;
    quantity: number;
    pictureUrl: string;
    brand: string;
    type: string;
}

export class Cart implements CartType
{
    id = nanoid(); // Generate a unique ID for the cart using nanoid
    items: CartItem[] = []; // Initialize an empty array for cart items
}