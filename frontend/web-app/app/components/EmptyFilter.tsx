'use client'
import { UseParamsStore } from '@/hooks/UseParamsStore';
import React from 'react'
import Heading from './Heading';
import { Button } from 'flowbite-react';
import { signIn } from 'next-auth/react';

type Props = {
    title?:string;
    subtitle?:string;
    showReset?:boolean
    showLogin?:boolean
    callBackUrl?:string
}

export default function EmptyFilter({
    title = 'No matches for this filter',
    subtitle = 'Try changing the filter or search term',
    showReset,
    showLogin,
    callBackUrl
}:Props) {
    const reset = UseParamsStore(state=>state.reset);
  return (
    <div className='flex flex-col gap-2 items-center justify-center h-[40vh] shadow-lg'>
        <Heading title={title} subtitle={subtitle} center/>
        <div className='mt-4'>
            {showReset && (
                <Button outline onClick={reset}>
                    Remove filters
                </Button>
            )}
            {showLogin && (
                <Button outline onClick={()=>signIn('id-server',{redirectTo:callBackUrl})}>
                    Login
                </Button>
            )}
        </div>
    </div>
  )
}
