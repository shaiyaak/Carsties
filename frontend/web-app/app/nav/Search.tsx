'use client'
import React, { useState } from 'react'
import {FaSearch} from 'react-icons/fa'
import { UseParamsStore } from '@/hooks/UseParamsStore'
export default function Search() {
  const setParams = UseParamsStore(state=>state.setParams)
  const setSearchValue = UseParamsStore(state=>state.setSearchValue)
  const searchValue = UseParamsStore(state=>state.searchValue)
  
  function onChange(event:any)
  {
    setSearchValue(event.target.value);
  }
  function search()
  {
    setParams({searchTerm:searchValue});
  }
  return (
    <div className='flex w-[50%] items-center vorder-2 rounded-full py-2 shadow-sm'>
        <input type="text"
        onKeyDown={(e:any)=>{
          if (e.key=="Enter") search()
        }}
        value={searchValue}
        onChange={onChange}
        placeholder='Search cars by make,model or color'
        className='
        flex-grow
        pl-5
        bg-transparent
        focus:outline-none
        border-transparent
        focus:border-transparent
        focus:ring-0
        text-sm
        text-gray-600
        '
        />
        <button onClick={search}>
            <FaSearch 
              size={34} 
              className='bg-red-400 text-white rounded-full p-2 cursor-pointer mx-2'/>
        </button>
    </div>
  )
}
