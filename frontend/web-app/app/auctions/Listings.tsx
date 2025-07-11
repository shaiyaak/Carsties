'use client'
import React, { useEffect, useState } from 'react'
import AuctionCard from './AuctionCard';
import AppPagination from '../components/AppPagination';
import { getData } from '../actions/auctionActions';
import Filters from './Filters';
import { UseParamsStore } from '@/hooks/UseParamsStore';
import {useShallow} from 'zustand/react/shallow'
import qs from 'query-string'
import EmptyFilter from '../components/EmptyFilter';
import { useAuctionStore } from '@/hooks/useAuctionStore';

export default  function Listings() {
const [loading, setLoading] = useState(true);
const params = UseParamsStore(useShallow(state=>({
  pageNumber:state.pageNumber,
  pageSize:state.pageSize,
  searchTerm:state.searchTerm,
  orderBy:state.orderBy,
  filterBy:state.filterBy,
  seller:state.seller,
  winner:state.winner
})))
const data = useAuctionStore(useShallow(state => ({
        auctions: state.auctions,
        totalCount: state.totalCount,
        pageCount: state.pageCount
    })))
const setData = useAuctionStore(state => state.setData);
const setParams = UseParamsStore(state=>state.setParams)
const url = qs.stringifyUrl({url:'',query:params})
function setPageNumber(pageNumber:number){
   setParams({pageNumber})
}

  useEffect(()=>{
    getData(url).then(data=>{
      console.log(url)
      setData(data)
      setLoading(false)
    })
  },[url,setData])

  if (loading) return <h3>Loading...</h3>
  return (
    <>
      <Filters/>
      {data.totalCount===0 ? (
        <EmptyFilter showReset/>
      ):(
        <>
        <div className='grid grid-cols-4 gap-6'>
          {data && data.auctions.map(auction=>(
              <AuctionCard auction={auction} key={auction.id}/>
          ))}
      </div>
      <div className='flex justify-center mt-4'>
        <AppPagination pageChanged={setPageNumber} 
          currentPage={params.pageNumber} pageCount={data.pageCount}/>
      </div>
      </>
      )}
    </>
  )
}
