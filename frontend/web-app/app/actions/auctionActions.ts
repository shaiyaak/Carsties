'use server'
import {PagedResult,Auction} from "@/types"
export async function getData(query:string) : Promise<PagedResult<Auction>> {
    const url = `http://localhost:6001/search${query}`;
    console.log(url);
    const res = await fetch(url, {
        cache: 'force-cache', // Default for static fetch
      });
    if (!res.ok) throw new Error(`Failed to fetch data with status ${res.status}`)
    return res.json();
}